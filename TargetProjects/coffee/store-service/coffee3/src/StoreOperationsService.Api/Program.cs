using Microsoft.EntityFrameworkCore;
using StoreOperationsService.Domain;
using StoreOperationsService.Domain.Config;
using StoreOperationsService.Domain.Exceptions;
using StoreOperationsService.Domain.Messaging;
using StoreOperationsService.Domain.Queue;
using StoreOperationsService.Domain.Repositories;
using StoreOperationsService.Domain.Services;
using StoreOperationsService.Infrastructure;
using StoreOperationsService.Infrastructure.Messaging;
using StoreOperationsService.Infrastructure.Queue;
using StoreOperationsService.Infrastructure.Repositories;
using StoreOperationsService.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHealthChecks();

builder.Services.AddDbContext<StoreOperationsDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("StoreOperationsDb")
            ?? "Server=(localdb)\\mssqllocaldb;Database=StoreOperationsDb;Trusted_Connection=True;",
        sql => sql.MigrationsAssembly(typeof(StoreOperationsDbContext).Assembly.FullName)));

builder.Services.AddSingleton<IEventBusAdapter, InMemoryEventBusAdapter>();
builder.Services.AddSingleton<IRabbitMqConsumerAdapter, InMemoryConsumerAdapter>();
builder.Services.AddScoped<IStoreOrderRepository, StoreOrderRepository>();
builder.Services.AddScoped<IOutboxRepository, OutboxRepository>();
builder.Services.AddQueueReadModel();
builder.Services.AddScoped<IOrderIntakeConsumer, OrderIntakeConsumer>();
builder.Services.AddSingleton<RushDesignationService>();
builder.Services.Configure<AtRiskOptions>(
    builder.Configuration.GetSection(AtRiskOptions.Section));
builder.Services.AddHostedService<OutboxPublisherService>();
builder.Services.AddHostedService<OrderIntakeSubscriber>();
builder.Services.AddHostedService<AtRiskBackgroundService>();

var app = builder.Build();

app.MapHealthChecks("/health");

// ── Rush designation endpoints (S2.2) ──────────────────────────────────────

app.MapPost("/store-operations/orders/{orderId:guid}/rush",
    async (
        Guid orderId,
        RushDesignateRequest? body,
        IStoreOrderRepository repo,
        IQueueReadModel queueReadModel,
        RushDesignationService rushService,
        HttpContext http,
        CancellationToken ct) =>
    {
        // Role guard: ShiftLead only
        var role = http.Request.Headers["X-User-Role"].FirstOrDefault();
        if (!string.Equals(role, "ShiftLead", StringComparison.OrdinalIgnoreCase))
            return Results.StatusCode(403);

        var snapshot = await repo.GetSnapshotAsync(orderId, ct);
        if (snapshot is null)
            return Results.NotFound();

        var entry = await queueReadModel.GetEntryAsync(orderId, ct);
        var storeId = entry?.StoreId ?? string.Empty;

        var lifecycleState = Enum.Parse<OrderLifecycleState>(snapshot.CurrentState);
        var priorityBand   = Enum.Parse<PriorityBand>(snapshot.PriorityBand);
        var modifiers      = new OperationalModifiers(snapshot.IsRush, snapshot.IsAtRisk, priorityBand);
        var order          = StoreOrder.Reconstitute(snapshot.OrderId, Guid.Empty, lifecycleState, modifiers, snapshot.CreatedAt, snapshot.UpdatedAt);

        var designatedBy = body?.DesignatedBy ?? "unknown";

        try
        {
            await rushService.DesignateRushAsync(order, storeId, designatedBy, queueReadModel, ct);
        }
        catch (InvalidOperationException ex)
        {
            return Results.Problem(ex.Message, statusCode: 400);
        }
        catch (DomainException ex) when (ex.ErrorCode == "RUSH_LIMIT_EXCEEDED")
        {
            return Results.Conflict(new { error = ex.ErrorCode, detail = ex.Message });
        }

        var updated = snapshot with
        {
            IsRush       = order.OperationalModifiers.IsRush,
            PriorityBand = order.OperationalModifiers.PriorityBand.ToString(),
            UpdatedAt    = order.UpdatedAt,
        };
        await repo.UpsertSnapshotAsync(updated, ct);
        await repo.AppendTransitionLogAsync(orderId, snapshot.CurrentState, snapshot.CurrentState,
            order.UpdatedAt, ct);

        return Results.NoContent();
    });

app.MapDelete("/store-operations/orders/{orderId:guid}/rush",
    async (
        Guid orderId,
        IStoreOrderRepository repo,
        IQueueReadModel queueReadModel,
        RushDesignationService rushService,
        HttpContext http,
        CancellationToken ct) =>
    {
        // Role guard: ShiftLead only
        var role = http.Request.Headers["X-User-Role"].FirstOrDefault();
        if (!string.Equals(role, "ShiftLead", StringComparison.OrdinalIgnoreCase))
            return Results.StatusCode(403);

        var snapshot = await repo.GetSnapshotAsync(orderId, ct);
        if (snapshot is null)
            return Results.NotFound();

        var lifecycleState = Enum.Parse<OrderLifecycleState>(snapshot.CurrentState);
        var priorityBand   = Enum.Parse<PriorityBand>(snapshot.PriorityBand);
        var modifiers      = new OperationalModifiers(snapshot.IsRush, snapshot.IsAtRisk, priorityBand);
        var order          = StoreOrder.Reconstitute(snapshot.OrderId, Guid.Empty, lifecycleState, modifiers, snapshot.CreatedAt, snapshot.UpdatedAt);

        await rushService.RemoveRushDesignationAsync(order, "shift-lead", queueReadModel, ct);

        var updated = snapshot with
        {
            IsRush       = order.OperationalModifiers.IsRush,
            PriorityBand = order.OperationalModifiers.PriorityBand.ToString(),
            UpdatedAt    = order.UpdatedAt,
        };
        await repo.UpsertSnapshotAsync(updated, ct);

        return Results.NoContent();
    });

app.Run();
public partial class Program { }

/// <summary>Request body for POST …/rush.</summary>
public sealed record RushDesignateRequest(string? DesignatedBy);


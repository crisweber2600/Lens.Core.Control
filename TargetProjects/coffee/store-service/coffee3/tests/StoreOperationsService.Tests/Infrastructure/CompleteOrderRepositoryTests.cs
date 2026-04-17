using Microsoft.EntityFrameworkCore;
using StoreOperationsService.Domain.Repositories;
using StoreOperationsService.Infrastructure;
using StoreOperationsService.Infrastructure.Repositories;

namespace StoreOperationsService.Tests.Infrastructure;

public sealed class CompleteOrderRepositoryTests
{
    private static StoreOperationsDbContext CreateInMemoryContext(string dbName)
    {
        var options = new DbContextOptionsBuilder<StoreOperationsDbContext>()
            .UseInMemoryDatabase(dbName)
            .Options;
        return new StoreOperationsDbContext(options);
    }

    private static StoreOrderSnapshotData MakeSnapshot(Guid? orderId = null, string state = "Completed") =>
        new(
            orderId ?? Guid.NewGuid(),
            state,
            "Standard",
            IsRush: false,
            IsAtRisk: false,
            CreatedAt: DateTimeOffset.UtcNow.AddMinutes(-5),
            UpdatedAt: DateTimeOffset.UtcNow);

    // ── Insert path ────────────────────────────────────────────────────────

    [Fact]
    public async Task CompleteOrderAsync_InsertsSnapshot()
    {
        await using var ctx = CreateInMemoryContext(nameof(CompleteOrderAsync_InsertsSnapshot));
        var repo = new StoreOrderRepository(ctx);
        var snapshot = MakeSnapshot();
        var now = DateTimeOffset.UtcNow;

        await repo.CompleteOrderAsync(snapshot, "StoreOrderCompleted", "{}", now);

        var stored = await ctx.StoreOrderSnapshots.FindAsync(snapshot.OrderId);
        Assert.NotNull(stored);
        Assert.Equal("Completed", stored!.CurrentState);
    }

    [Fact]
    public async Task CompleteOrderAsync_AppendsTransitionLog()
    {
        await using var ctx = CreateInMemoryContext(nameof(CompleteOrderAsync_AppendsTransitionLog));
        var repo = new StoreOrderRepository(ctx);
        var snapshot = MakeSnapshot();
        var now = DateTimeOffset.UtcNow;

        await repo.CompleteOrderAsync(snapshot, "StoreOrderCompleted", "{}", now);

        var log = await ctx.OrderTransitionLogs
            .FirstOrDefaultAsync(l => l.OrderId == snapshot.OrderId);
        Assert.NotNull(log);
        Assert.Equal("Ready", log!.FromState);
        Assert.Equal("Completed", log.ToState);
        Assert.Equal(now, log.OccurredAt);
    }

    [Fact]
    public async Task CompleteOrderAsync_WritesOutboxMessage()
    {
        await using var ctx = CreateInMemoryContext(nameof(CompleteOrderAsync_WritesOutboxMessage));
        var repo = new StoreOrderRepository(ctx);
        var snapshot = MakeSnapshot();
        var now = DateTimeOffset.UtcNow;
        const string payload = "{\"orderId\":\"test\"}";

        await repo.CompleteOrderAsync(snapshot, "StoreOrderCompleted", payload, now, "corr-1");

        var msg = await ctx.OutboxMessages.FirstOrDefaultAsync();
        Assert.NotNull(msg);
        Assert.Equal("StoreOrderCompleted", msg!.Type);
        Assert.Equal(payload, msg.Payload);
        Assert.Equal(now, msg.OccurredAt);
        Assert.Equal("corr-1", msg.CorrelationId);
        Assert.Null(msg.SentAt);
    }

    [Fact]
    public async Task CompleteOrderAsync_AllThreeRecordsPersistedTogether()
    {
        await using var ctx = CreateInMemoryContext(nameof(CompleteOrderAsync_AllThreeRecordsPersistedTogether));
        var repo = new StoreOrderRepository(ctx);
        var snapshot = MakeSnapshot();
        var now = DateTimeOffset.UtcNow;

        await repo.CompleteOrderAsync(snapshot, "StoreOrderCompleted", "{}", now);

        Assert.Equal(1, await ctx.StoreOrderSnapshots.CountAsync());
        Assert.Equal(1, await ctx.OrderTransitionLogs.CountAsync());
        Assert.Equal(1, await ctx.OutboxMessages.CountAsync());
    }

    // ── Update path ────────────────────────────────────────────────────────

    [Fact]
    public async Task CompleteOrderAsync_UpdatesExistingSnapshot()
    {
        await using var ctx = CreateInMemoryContext(nameof(CompleteOrderAsync_UpdatesExistingSnapshot));
        var repo = new StoreOrderRepository(ctx);

        var orderId = Guid.NewGuid();
        // Seed a snapshot at Ready state
        await repo.UpsertSnapshotAsync(new StoreOrderSnapshotData(
            orderId, "Ready", "Standard", false, false,
            DateTimeOffset.UtcNow.AddMinutes(-10), DateTimeOffset.UtcNow.AddMinutes(-1)));

        var snapshot = MakeSnapshot(orderId, "Completed");
        var now = DateTimeOffset.UtcNow;

        await repo.CompleteOrderAsync(snapshot, "StoreOrderCompleted", "{}", now);

        var stored = await ctx.StoreOrderSnapshots.FindAsync(orderId);
        Assert.Equal("Completed", stored!.CurrentState);
    }

    [Fact]
    public async Task CompleteOrderAsync_CorrelationId_NullByDefault()
    {
        await using var ctx = CreateInMemoryContext(nameof(CompleteOrderAsync_CorrelationId_NullByDefault));
        var repo = new StoreOrderRepository(ctx);
        var snapshot = MakeSnapshot();
        var now = DateTimeOffset.UtcNow;

        await repo.CompleteOrderAsync(snapshot, "StoreOrderCompleted", "{}", now);

        var msg = await ctx.OutboxMessages.FirstOrDefaultAsync();
        Assert.Null(msg!.CorrelationId);
    }
}

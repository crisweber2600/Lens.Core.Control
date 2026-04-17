using Microsoft.EntityFrameworkCore;
using StoreOperationsService.Domain.Repositories;
using StoreOperationsService.Infrastructure;
using StoreOperationsService.Infrastructure.Repositories;

namespace StoreOperationsService.Tests.Infrastructure;

public sealed class CancelOrderRepositoryTests
{
    private static StoreOperationsDbContext CreateInMemoryContext(string dbName)
    {
        var options = new DbContextOptionsBuilder<StoreOperationsDbContext>()
            .UseInMemoryDatabase(dbName)
            .Options;
        return new StoreOperationsDbContext(options);
    }

    private static StoreOrderSnapshotData MakeSnapshot(Guid? orderId = null, string state = "Cancelled") =>
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
    public async Task CancelOrderAsync_InsertsSnapshot()
    {
        await using var ctx = CreateInMemoryContext(nameof(CancelOrderAsync_InsertsSnapshot));
        var repo = new StoreOrderRepository(ctx);
        var snapshot = MakeSnapshot();
        var now = DateTimeOffset.UtcNow;

        await repo.CancelOrderAsync(snapshot, "Received", "StoreOrderCancelled", "{}", now);

        var stored = await ctx.StoreOrderSnapshots.FindAsync(snapshot.OrderId);
        Assert.NotNull(stored);
        Assert.Equal("Cancelled", stored!.CurrentState);
    }

    [Fact]
    public async Task CancelOrderAsync_AppendsTransitionLog()
    {
        await using var ctx = CreateInMemoryContext(nameof(CancelOrderAsync_AppendsTransitionLog));
        var repo = new StoreOrderRepository(ctx);
        var snapshot = MakeSnapshot();
        var now = DateTimeOffset.UtcNow;

        await repo.CancelOrderAsync(snapshot, "Queued", "StoreOrderCancelled", "{}", now);

        var log = await ctx.OrderTransitionLogs
            .FirstOrDefaultAsync(l => l.OrderId == snapshot.OrderId);
        Assert.NotNull(log);
        Assert.Equal(now, log!.OccurredAt);
    }

    [Fact]
    public async Task CancelOrderAsync_TransitionLog_FromStateMatchesParam()
    {
        await using var ctx = CreateInMemoryContext(nameof(CancelOrderAsync_TransitionLog_FromStateMatchesParam));
        var repo = new StoreOrderRepository(ctx);
        var snapshot = MakeSnapshot();
        var now = DateTimeOffset.UtcNow;

        await repo.CancelOrderAsync(snapshot, "InProgress", "StoreOrderCancelled", "{}", now);

        var log = await ctx.OrderTransitionLogs.FirstOrDefaultAsync();
        Assert.Equal("InProgress", log!.FromState);
    }

    [Fact]
    public async Task CancelOrderAsync_TransitionLog_ToStateIsCancelled()
    {
        await using var ctx = CreateInMemoryContext(nameof(CancelOrderAsync_TransitionLog_ToStateIsCancelled));
        var repo = new StoreOrderRepository(ctx);
        var snapshot = MakeSnapshot();
        var now = DateTimeOffset.UtcNow;

        await repo.CancelOrderAsync(snapshot, "Ready", "StoreOrderCancelled", "{}", now);

        var log = await ctx.OrderTransitionLogs.FirstOrDefaultAsync();
        Assert.Equal("Cancelled", log!.ToState);
    }

    [Fact]
    public async Task CancelOrderAsync_WritesOutboxMessage()
    {
        await using var ctx = CreateInMemoryContext(nameof(CancelOrderAsync_WritesOutboxMessage));
        var repo = new StoreOrderRepository(ctx);
        var snapshot = MakeSnapshot();
        var now = DateTimeOffset.UtcNow;
        const string payload = "{\"orderId\":\"test\",\"reason\":\"CustomerRequest\"}";

        await repo.CancelOrderAsync(snapshot, "Received", "StoreOrderCancelled", payload, now, "corr-cancel-1");

        var msg = await ctx.OutboxMessages.FirstOrDefaultAsync();
        Assert.NotNull(msg);
        Assert.Equal("StoreOrderCancelled", msg!.Type);
        Assert.Equal(payload, msg.Payload);
        Assert.Equal(now, msg.OccurredAt);
        Assert.Equal("corr-cancel-1", msg.CorrelationId);
        Assert.Null(msg.SentAt);
    }

    [Fact]
    public async Task CancelOrderAsync_AllThreeRecordsPersistedTogether()
    {
        await using var ctx = CreateInMemoryContext(nameof(CancelOrderAsync_AllThreeRecordsPersistedTogether));
        var repo = new StoreOrderRepository(ctx);
        var snapshot = MakeSnapshot();
        var now = DateTimeOffset.UtcNow;

        await repo.CancelOrderAsync(snapshot, "Queued", "StoreOrderCancelled", "{}", now);

        Assert.Equal(1, await ctx.StoreOrderSnapshots.CountAsync());
        Assert.Equal(1, await ctx.OrderTransitionLogs.CountAsync());
        Assert.Equal(1, await ctx.OutboxMessages.CountAsync());
    }

    // ── Update path ────────────────────────────────────────────────────────

    [Fact]
    public async Task CancelOrderAsync_UpdatesExistingSnapshot()
    {
        await using var ctx = CreateInMemoryContext(nameof(CancelOrderAsync_UpdatesExistingSnapshot));
        var repo = new StoreOrderRepository(ctx);

        var orderId = Guid.NewGuid();
        await repo.UpsertSnapshotAsync(new StoreOrderSnapshotData(
            orderId, "InProgress", "Standard", false, false,
            DateTimeOffset.UtcNow.AddMinutes(-10), DateTimeOffset.UtcNow.AddMinutes(-1)));

        var snapshot = MakeSnapshot(orderId, "Cancelled");
        var now = DateTimeOffset.UtcNow;

        await repo.CancelOrderAsync(snapshot, "InProgress", "StoreOrderCancelled", "{}", now);

        var stored = await ctx.StoreOrderSnapshots.FindAsync(orderId);
        Assert.Equal("Cancelled", stored!.CurrentState);
    }

    [Fact]
    public async Task CancelOrderAsync_WithNullCorrelationId_WritesNullCorrelationId()
    {
        await using var ctx = CreateInMemoryContext(nameof(CancelOrderAsync_WithNullCorrelationId_WritesNullCorrelationId));
        var repo = new StoreOrderRepository(ctx);
        var snapshot = MakeSnapshot();
        var now = DateTimeOffset.UtcNow;

        await repo.CancelOrderAsync(snapshot, "Received", "StoreOrderCancelled", "{}", now, correlationId: null);

        var msg = await ctx.OutboxMessages.FirstOrDefaultAsync();
        Assert.Null(msg!.CorrelationId);
    }

    // ── Concurrency guard ──────────────────────────────────────────────────

    [Fact]
    public async Task CancelOrderAsync_StaleRowVersion_ThrowsDbUpdateConcurrencyException()
    {
        await using var ctx = CreateInMemoryContext(nameof(CancelOrderAsync_StaleRowVersion_ThrowsDbUpdateConcurrencyException));
        var repo = new StoreOrderRepository(ctx);
        var orderId = Guid.NewGuid();

        // Seed snapshot at RowVersion 0
        await repo.UpsertSnapshotAsync(new StoreOrderSnapshotData(
            orderId, "Received", "Standard", false, false,
            DateTimeOffset.UtcNow.AddMinutes(-5), DateTimeOffset.UtcNow.AddMinutes(-1)));

        // Advance RowVersion by upsert
        await repo.UpsertSnapshotAsync(new StoreOrderSnapshotData(
            orderId, "Queued", "Standard", false, false,
            DateTimeOffset.UtcNow.AddMinutes(-5), DateTimeOffset.UtcNow));

        // Attempt to cancel with stale RowVersion = 0 (entity is now at 1)
        var staleSnapshot = new StoreOrderSnapshotData(
            orderId, "Cancelled", "Standard", false, false,
            DateTimeOffset.UtcNow.AddMinutes(-5), DateTimeOffset.UtcNow, RowVersion: 0);

        await Assert.ThrowsAsync<DbUpdateConcurrencyException>(() =>
            repo.CancelOrderAsync(staleSnapshot, "Queued", "StoreOrderCancelled", "{}", DateTimeOffset.UtcNow));
    }
}

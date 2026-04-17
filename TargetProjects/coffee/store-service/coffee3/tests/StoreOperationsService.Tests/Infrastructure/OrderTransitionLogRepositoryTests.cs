using Microsoft.EntityFrameworkCore;
using StoreOperationsService.Domain.Repositories;
using StoreOperationsService.Infrastructure;
using StoreOperationsService.Infrastructure.Repositories;

namespace StoreOperationsService.Tests.Infrastructure;

public sealed class OrderTransitionLogRepositoryTests
{
    private static StoreOperationsDbContext CreateInMemoryContext(string dbName)
    {
        var options = new DbContextOptionsBuilder<StoreOperationsDbContext>()
            .UseInMemoryDatabase(dbName)
            .Options;
        return new StoreOperationsDbContext(options);
    }

    [Fact]
    public async Task AppendTransitionLogAsync_CreatesLogEntry()
    {
        await using var ctx = CreateInMemoryContext(nameof(AppendTransitionLogAsync_CreatesLogEntry));
        var repo = new StoreOrderRepository(ctx);
        var orderId = Guid.NewGuid();
        var now = DateTimeOffset.UtcNow;

        await repo.AppendTransitionLogAsync(orderId, "Received", "Queued", now);

        var log = await ctx.OrderTransitionLogs.SingleAsync();
        Assert.Equal(orderId, log.OrderId);
        Assert.Equal("Received", log.FromState);
        Assert.Equal("Queued", log.ToState);
        Assert.Equal(now, log.OccurredAt);
    }

    [Fact]
    public async Task AppendTransitionLogAsync_MultipleEntriesSameOrder()
    {
        await using var ctx = CreateInMemoryContext(nameof(AppendTransitionLogAsync_MultipleEntriesSameOrder));
        var repo = new StoreOrderRepository(ctx);
        var orderId = Guid.NewGuid();
        var t = DateTimeOffset.UtcNow;

        await repo.AppendTransitionLogAsync(orderId, "Received", "Queued",     t);
        await repo.AppendTransitionLogAsync(orderId, "Queued",   "InProgress", t.AddSeconds(1));
        await repo.AppendTransitionLogAsync(orderId, "InProgress","Ready",     t.AddSeconds(2));

        Assert.Equal(3, await ctx.OrderTransitionLogs.CountAsync());
    }

    [Fact]
    public async Task AppendTransitionLogAsync_DifferentOrders_StoredIndependently()
    {
        await using var ctx = CreateInMemoryContext(nameof(AppendTransitionLogAsync_DifferentOrders_StoredIndependently));
        var repo = new StoreOrderRepository(ctx);
        var t = DateTimeOffset.UtcNow;

        await repo.AppendTransitionLogAsync(Guid.NewGuid(), "Received", "Queued", t);
        await repo.AppendTransitionLogAsync(Guid.NewGuid(), "Received", "Queued", t);

        Assert.Equal(2, await ctx.OrderTransitionLogs.CountAsync());
    }

    [Fact]
    public async Task AppendTransitionLogAsync_PreservesInsertionOrder()
    {
        await using var ctx = CreateInMemoryContext(nameof(AppendTransitionLogAsync_PreservesInsertionOrder));
        var repo = new StoreOrderRepository(ctx);
        var orderId = Guid.NewGuid();
        var t = DateTimeOffset.UtcNow;

        await repo.AppendTransitionLogAsync(orderId, "Received",   "Queued",     t);
        await repo.AppendTransitionLogAsync(orderId, "Queued",     "InProgress", t.AddSeconds(1));

        var logs = await ctx.OrderTransitionLogs
            .Where(l => l.OrderId == orderId)
            .OrderBy(l => l.Id)
            .ToListAsync();

        Assert.Equal("Received",   logs[0].FromState);
        Assert.Equal("Queued",     logs[0].ToState);
        Assert.Equal("Queued",     logs[1].FromState);
        Assert.Equal("InProgress", logs[1].ToState);
    }

    [Fact]
    public async Task AppendTransitionLogAsync_DoesNotAffectSnapshotTable()
    {
        await using var ctx = CreateInMemoryContext(nameof(AppendTransitionLogAsync_DoesNotAffectSnapshotTable));
        var repo = new StoreOrderRepository(ctx);

        await repo.AppendTransitionLogAsync(Guid.NewGuid(), "Received", "Queued", DateTimeOffset.UtcNow);

        Assert.Equal(0, await ctx.StoreOrderSnapshots.CountAsync());
    }

    [Fact]
    public async Task AppendTransitionLogAsync_DoesNotAffectExistingSnapshot()
    {
        await using var ctx = CreateInMemoryContext(nameof(AppendTransitionLogAsync_DoesNotAffectExistingSnapshot));
        var repo = new StoreOrderRepository(ctx);
        var orderId = Guid.NewGuid();
        var now = DateTimeOffset.UtcNow;

        await repo.UpsertSnapshotAsync(new StoreOrderSnapshotData(
            orderId, "Received", "Standard", false, false, now, now));

        await repo.AppendTransitionLogAsync(orderId, "Received", "Queued", now);

        var snapshot = await ctx.StoreOrderSnapshots.FindAsync(orderId);
        Assert.Equal("Received", snapshot!.CurrentState);
    }
}

using Microsoft.EntityFrameworkCore;
using StoreOperationsService.Infrastructure;
using StoreOperationsService.Infrastructure.Entities;

namespace StoreOperationsService.Tests.Infrastructure;

public sealed class StoreOperationsDbContextTests
{
    private static StoreOperationsDbContext CreateInMemoryContext(string dbName)
    {
        var options = new DbContextOptionsBuilder<StoreOperationsDbContext>()
            .UseInMemoryDatabase(dbName)
            .Options;
        return new StoreOperationsDbContext(options);
    }

    [Fact]
    public async Task DbContext_CanBeCreated()
    {
        await using var ctx = CreateInMemoryContext(nameof(DbContext_CanBeCreated));
        Assert.NotNull(ctx);
    }

    [Fact]
    public async Task StoreOrderSnapshots_CanAddAndRetrieve()
    {
        var orderId = Guid.NewGuid();
        await using var ctx = CreateInMemoryContext(nameof(StoreOrderSnapshots_CanAddAndRetrieve));

        ctx.StoreOrderSnapshots.Add(new StoreOrderSnapshot
        {
            OrderId = orderId,
            CurrentState = "Queued",
            PriorityBand = "Standard",
            IsRush = false,
            IsAtRisk = false,
            CreatedAt = DateTimeOffset.UtcNow,
            UpdatedAt = DateTimeOffset.UtcNow
        });
        await ctx.SaveChangesAsync();

        var snapshot = await ctx.StoreOrderSnapshots.FindAsync(orderId);
        Assert.NotNull(snapshot);
        Assert.Equal("Queued", snapshot.CurrentState);
        Assert.Equal("Standard", snapshot.PriorityBand);
    }

    [Fact]
    public async Task OrderTransitionLogs_CanAddAndRetrieve()
    {
        var orderId = Guid.NewGuid();
        await using var ctx = CreateInMemoryContext(nameof(OrderTransitionLogs_CanAddAndRetrieve));

        ctx.OrderTransitionLogs.Add(new OrderTransitionLog
        {
            OrderId = orderId,
            FromState = "Received",
            ToState = "Queued",
            OccurredAt = DateTimeOffset.UtcNow
        });
        await ctx.SaveChangesAsync();

        var logs = await ctx.OrderTransitionLogs
            .Where(l => l.OrderId == orderId)
            .ToListAsync();

        Assert.Single(logs);
        Assert.Equal("Received", logs[0].FromState);
        Assert.Equal("Queued", logs[0].ToState);
    }

    [Fact]
    public async Task OrderTransitionLogs_CanAppendMultiple()
    {
        var orderId = Guid.NewGuid();
        await using var ctx = CreateInMemoryContext(nameof(OrderTransitionLogs_CanAppendMultiple));

        var transitions = new[]
        {
            ("Received", "Queued"),
            ("Queued", "InProgress"),
            ("InProgress", "Ready")
        };

        foreach (var (from, to) in transitions)
        {
            ctx.OrderTransitionLogs.Add(new OrderTransitionLog
            {
                OrderId = orderId,
                FromState = from,
                ToState = to,
                OccurredAt = DateTimeOffset.UtcNow
            });
        }
        await ctx.SaveChangesAsync();

        var count = await ctx.OrderTransitionLogs
            .CountAsync(l => l.OrderId == orderId);
        Assert.Equal(3, count);
    }

    [Fact]
    public async Task StoreOrderSnapshot_UpdatedAt_CanBeModified()
    {
        var orderId = Guid.NewGuid();
        var initial = DateTimeOffset.UtcNow.AddMinutes(-5);
        await using var ctx = CreateInMemoryContext(nameof(StoreOrderSnapshot_UpdatedAt_CanBeModified));

        ctx.StoreOrderSnapshots.Add(new StoreOrderSnapshot
        {
            OrderId = orderId,
            CurrentState = "Queued",
            PriorityBand = "Standard",
            CreatedAt = initial,
            UpdatedAt = initial
        });
        await ctx.SaveChangesAsync();

        var snapshot = await ctx.StoreOrderSnapshots.FindAsync(orderId);
        Assert.NotNull(snapshot);
        var updated = DateTimeOffset.UtcNow;
        snapshot.CurrentState = "InProgress";
        snapshot.UpdatedAt = updated;
        await ctx.SaveChangesAsync();

        var reloaded = await ctx.StoreOrderSnapshots.FindAsync(orderId);
        Assert.Equal("InProgress", reloaded!.CurrentState);
        Assert.Equal(updated, reloaded.UpdatedAt);
    }

    [Fact]
    public async Task StoreOrderSnapshot_IsRush_DefaultsFalse()
    {
        var orderId = Guid.NewGuid();
        await using var ctx = CreateInMemoryContext(nameof(StoreOrderSnapshot_IsRush_DefaultsFalse));

        ctx.StoreOrderSnapshots.Add(new StoreOrderSnapshot
        {
            OrderId = orderId,
            CurrentState = "Received",
            PriorityBand = "Standard",
            CreatedAt = DateTimeOffset.UtcNow,
            UpdatedAt = DateTimeOffset.UtcNow
        });
        await ctx.SaveChangesAsync();

        var snapshot = await ctx.StoreOrderSnapshots.FindAsync(orderId);
        Assert.False(snapshot!.IsRush);
        Assert.False(snapshot.IsAtRisk);
    }

    [Fact]
    public async Task StoreOrderSnapshot_IsRush_CanBeSet()
    {
        var orderId = Guid.NewGuid();
        await using var ctx = CreateInMemoryContext(nameof(StoreOrderSnapshot_IsRush_CanBeSet));

        ctx.StoreOrderSnapshots.Add(new StoreOrderSnapshot
        {
            OrderId = orderId,
            CurrentState = "Queued",
            PriorityBand = "Rush",
            IsRush = true,
            CreatedAt = DateTimeOffset.UtcNow,
            UpdatedAt = DateTimeOffset.UtcNow
        });
        await ctx.SaveChangesAsync();

        var snapshot = await ctx.StoreOrderSnapshots.FindAsync(orderId);
        Assert.True(snapshot!.IsRush);
        Assert.Equal("Rush", snapshot.PriorityBand);
    }

    [Fact]
    public async Task StoreOrderSnapshot_IsAtRisk_CanBeSet()
    {
        var orderId = Guid.NewGuid();
        await using var ctx = CreateInMemoryContext(nameof(StoreOrderSnapshot_IsAtRisk_CanBeSet));

        ctx.StoreOrderSnapshots.Add(new StoreOrderSnapshot
        {
            OrderId = orderId,
            CurrentState = "Queued",
            PriorityBand = "Standard",
            IsAtRisk = true,
            CreatedAt = DateTimeOffset.UtcNow,
            UpdatedAt = DateTimeOffset.UtcNow
        });
        await ctx.SaveChangesAsync();

        var snapshot = await ctx.StoreOrderSnapshots.FindAsync(orderId);
        Assert.True(snapshot!.IsAtRisk);
    }

    [Fact]
    public async Task OrderTransitionLogs_CanQueryByOrderId()
    {
        var orderId1 = Guid.NewGuid();
        var orderId2 = Guid.NewGuid();
        await using var ctx = CreateInMemoryContext(nameof(OrderTransitionLogs_CanQueryByOrderId));

        ctx.OrderTransitionLogs.AddRange(
            new OrderTransitionLog { OrderId = orderId1, FromState = "Received", ToState = "Queued", OccurredAt = DateTimeOffset.UtcNow },
            new OrderTransitionLog { OrderId = orderId2, FromState = "Received", ToState = "Queued", OccurredAt = DateTimeOffset.UtcNow },
            new OrderTransitionLog { OrderId = orderId1, FromState = "Queued", ToState = "InProgress", OccurredAt = DateTimeOffset.UtcNow }
        );
        await ctx.SaveChangesAsync();

        var logsForOrder1 = await ctx.OrderTransitionLogs
            .Where(l => l.OrderId == orderId1)
            .ToListAsync();

        Assert.Equal(2, logsForOrder1.Count);
    }
}

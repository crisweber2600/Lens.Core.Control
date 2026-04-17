using Microsoft.EntityFrameworkCore;
using StoreOperationsService.Domain.Repositories;
using StoreOperationsService.Infrastructure;
using StoreOperationsService.Infrastructure.Repositories;

namespace StoreOperationsService.Tests.Infrastructure;

public sealed class StoreOrderRepositoryTests
{
    private static StoreOperationsDbContext CreateInMemoryContext(string dbName)
    {
        var options = new DbContextOptionsBuilder<StoreOperationsDbContext>()
            .UseInMemoryDatabase(dbName)
            .Options;
        return new StoreOperationsDbContext(options);
    }

    private static StoreOrderSnapshotData MakeSnapshot(Guid? orderId = null, string state = "Received") =>
        new(
            orderId ?? Guid.NewGuid(),
            state,
            "Standard",
            IsRush: false,
            IsAtRisk: false,
            CreatedAt: DateTimeOffset.UtcNow,
            UpdatedAt: DateTimeOffset.UtcNow);

    // ── UpsertSnapshotAsync — insert path ──────────────────────────────────

    [Fact]
    public async Task UpsertSnapshotAsync_InsertsNewSnapshot()
    {
        await using var ctx = CreateInMemoryContext(nameof(UpsertSnapshotAsync_InsertsNewSnapshot));
        var repo = new StoreOrderRepository(ctx);
        var snapshot = MakeSnapshot();

        await repo.UpsertSnapshotAsync(snapshot);

        var stored = await ctx.StoreOrderSnapshots.FindAsync(snapshot.OrderId);
        Assert.NotNull(stored);
        Assert.Equal(snapshot.CurrentState, stored!.CurrentState);
    }

    [Fact]
    public async Task UpsertSnapshotAsync_RowCountIncreasesOnInsert()
    {
        await using var ctx = CreateInMemoryContext(nameof(UpsertSnapshotAsync_RowCountIncreasesOnInsert));
        var repo = new StoreOrderRepository(ctx);

        await repo.UpsertSnapshotAsync(MakeSnapshot());
        await repo.UpsertSnapshotAsync(MakeSnapshot());

        Assert.Equal(2, await ctx.StoreOrderSnapshots.CountAsync());
    }

    // ── UpsertSnapshotAsync — update path ─────────────────────────────────

    [Fact]
    public async Task UpsertSnapshotAsync_UpdatesExistingState()
    {
        await using var ctx = CreateInMemoryContext(nameof(UpsertSnapshotAsync_UpdatesExistingState));
        var repo = new StoreOrderRepository(ctx);
        var id = Guid.NewGuid();

        await repo.UpsertSnapshotAsync(MakeSnapshot(id, "Received"));
        await repo.UpsertSnapshotAsync(MakeSnapshot(id, "Queued"));

        var stored = await ctx.StoreOrderSnapshots.FindAsync(id);
        Assert.Equal("Queued", stored!.CurrentState);
    }

    [Fact]
    public async Task UpsertSnapshotAsync_UpdateDoesNotDuplicateRow()
    {
        await using var ctx = CreateInMemoryContext(nameof(UpsertSnapshotAsync_UpdateDoesNotDuplicateRow));
        var repo = new StoreOrderRepository(ctx);
        var id = Guid.NewGuid();

        await repo.UpsertSnapshotAsync(MakeSnapshot(id, "Received"));
        await repo.UpsertSnapshotAsync(MakeSnapshot(id, "Queued"));

        Assert.Equal(1, await ctx.StoreOrderSnapshots.CountAsync());
    }

    [Fact]
    public async Task UpsertSnapshotAsync_UpdatesIsRush()
    {
        await using var ctx = CreateInMemoryContext(nameof(UpsertSnapshotAsync_UpdatesIsRush));
        var repo = new StoreOrderRepository(ctx);
        var id = Guid.NewGuid();

        await repo.UpsertSnapshotAsync(MakeSnapshot(id));
        await repo.UpsertSnapshotAsync(MakeSnapshot(id) with { IsRush = true });

        var stored = await ctx.StoreOrderSnapshots.FindAsync(id);
        Assert.True(stored!.IsRush);
    }

    [Fact]
    public async Task UpsertSnapshotAsync_UpdatesIsAtRisk()
    {
        await using var ctx = CreateInMemoryContext(nameof(UpsertSnapshotAsync_UpdatesIsAtRisk));
        var repo = new StoreOrderRepository(ctx);
        var id = Guid.NewGuid();

        await repo.UpsertSnapshotAsync(MakeSnapshot(id));
        await repo.UpsertSnapshotAsync(MakeSnapshot(id) with { IsAtRisk = true });

        var stored = await ctx.StoreOrderSnapshots.FindAsync(id);
        Assert.True(stored!.IsAtRisk);
    }

    [Fact]
    public async Task UpsertSnapshotAsync_UpdatesPriorityBand()
    {
        await using var ctx = CreateInMemoryContext(nameof(UpsertSnapshotAsync_UpdatesPriorityBand));
        var repo = new StoreOrderRepository(ctx);
        var id = Guid.NewGuid();

        await repo.UpsertSnapshotAsync(MakeSnapshot(id));
        await repo.UpsertSnapshotAsync(MakeSnapshot(id) with { PriorityBand = "Escalated" });

        var stored = await ctx.StoreOrderSnapshots.FindAsync(id);
        Assert.Equal("Escalated", stored!.PriorityBand);
    }

    // ── GetSnapshotAsync ───────────────────────────────────────────────────

    [Fact]
    public async Task GetSnapshotAsync_ReturnsNull_WhenNotFound()
    {
        await using var ctx = CreateInMemoryContext(nameof(GetSnapshotAsync_ReturnsNull_WhenNotFound));
        var repo = new StoreOrderRepository(ctx);

        var result = await repo.GetSnapshotAsync(Guid.NewGuid());

        Assert.Null(result);
    }

    [Fact]
    public async Task GetSnapshotAsync_ReturnsSnapshot_WhenFound()
    {
        await using var ctx = CreateInMemoryContext(nameof(GetSnapshotAsync_ReturnsSnapshot_WhenFound));
        var repo = new StoreOrderRepository(ctx);
        var snapshot = MakeSnapshot();
        await repo.UpsertSnapshotAsync(snapshot);

        var result = await repo.GetSnapshotAsync(snapshot.OrderId);

        Assert.NotNull(result);
        Assert.Equal(snapshot.OrderId, result!.OrderId);
    }

    [Fact]
    public async Task GetSnapshotAsync_ReturnsCorrectState()
    {
        await using var ctx = CreateInMemoryContext(nameof(GetSnapshotAsync_ReturnsCorrectState));
        var repo = new StoreOrderRepository(ctx);
        var id = Guid.NewGuid();
        await repo.UpsertSnapshotAsync(MakeSnapshot(id, "InProgress"));

        var result = await repo.GetSnapshotAsync(id);

        Assert.Equal("InProgress", result!.CurrentState);
    }

    [Fact]
    public async Task GetSnapshotAsync_ReturnsLatestState_AfterUpdate()
    {
        await using var ctx = CreateInMemoryContext(nameof(GetSnapshotAsync_ReturnsLatestState_AfterUpdate));
        var repo = new StoreOrderRepository(ctx);
        var id = Guid.NewGuid();

        await repo.UpsertSnapshotAsync(MakeSnapshot(id, "Received"));
        await repo.UpsertSnapshotAsync(MakeSnapshot(id, "Ready"));

        var result = await repo.GetSnapshotAsync(id);

        Assert.Equal("Ready", result!.CurrentState);
    }

    // ── Interface contract reflection ──────────────────────────────────────

    [Fact]
    public void StoreOrderRepository_ImplementsIStoreOrderRepository()
    {
        Assert.True(typeof(StoreOrderRepository)
            .IsAssignableTo(typeof(IStoreOrderRepository)));
    }
}

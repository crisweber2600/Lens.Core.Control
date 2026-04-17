using Microsoft.EntityFrameworkCore;
using StoreOperationsService.Domain.Repositories;
using StoreOperationsService.Infrastructure;
using StoreOperationsService.Infrastructure.Repositories;

namespace StoreOperationsService.Tests.Infrastructure;

/// <summary>
/// S1.4 – Ensures the optimistic-concurrency guard in StoreOrderRepository
/// prevents dirty writes when two callers hold stale RowVersions.
/// </summary>
public sealed class SnapshotConcurrencyGuardTests
{
    // ── helpers ──────────────────────────────────────────────────────────────

    private static StoreOperationsDbContext BuildContext()
    {
        var opts = new DbContextOptionsBuilder<StoreOperationsDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new StoreOperationsDbContext(opts);
    }

    private static StoreOrderSnapshotData MakeSnapshot(
        Guid orderId,
        string state = "Received",
        uint rowVersion = 0)
        => new(orderId, state, "Standard", false, false,
               DateTimeOffset.UtcNow, DateTimeOffset.UtcNow, rowVersion);

    // ── tests ─────────────────────────────────────────────────────────────────

    [Fact]
    public async Task Insert_WithRowVersionZero_Succeeds()
    {
        await using var ctx = BuildContext();
        var repo     = new StoreOrderRepository(ctx);
        var snapshot = MakeSnapshot(Guid.NewGuid(), rowVersion: 0);

        await repo.UpsertSnapshotAsync(snapshot);

        var stored = await repo.GetSnapshotAsync(snapshot.OrderId);
        Assert.NotNull(stored);
    }

    [Fact]
    public async Task FirstUpdate_WithMatchingRowVersion_Succeeds()
    {
        await using var ctx = BuildContext();
        var repo    = new StoreOrderRepository(ctx);
        var orderId = Guid.NewGuid();

        // Insert initial snapshot
        await repo.UpsertSnapshotAsync(MakeSnapshot(orderId, "Received", rowVersion: 0));

        // Read back to get current RowVersion (should be 0 after insert via InMemory)
        var first = await repo.GetSnapshotAsync(orderId);
        Assert.NotNull(first);

        // Update using the row version that was read
        var updated = first with { CurrentState = "Queued" };
        await repo.UpsertSnapshotAsync(updated); // should not throw
    }

    [Fact]
    public async Task SecondUpdate_WithStaleRowVersion_Throws()
    {
        await using var ctx = BuildContext();
        var repo    = new StoreOrderRepository(ctx);
        var orderId = Guid.NewGuid();

        // Insert
        await repo.UpsertSnapshotAsync(MakeSnapshot(orderId, "Received", rowVersion: 0));

        // First reader reads RowVersion = 0
        var readerA = await repo.GetSnapshotAsync(orderId);
        Assert.NotNull(readerA);

        // First writer updates successfully → RowVersion becomes 1
        var updatedByA = readerA with { CurrentState = "Queued" };
        await repo.UpsertSnapshotAsync(updatedByA);

        // Second writer still holds the stale RowVersion (0) → must be rejected
        var staleUpdate = readerA with { CurrentState = "InProgress" };
        await Assert.ThrowsAsync<DbUpdateConcurrencyException>(
            () => repo.UpsertSnapshotAsync(staleUpdate));
    }

    [Fact]
    public async Task GetSnapshotAsync_ReturnsRowVersion_MatchingStoredValue()
    {
        await using var ctx = BuildContext();
        var repo    = new StoreOrderRepository(ctx);
        var orderId = Guid.NewGuid();

        await repo.UpsertSnapshotAsync(MakeSnapshot(orderId, "Received", rowVersion: 0));
        var first = await repo.GetSnapshotAsync(orderId);
        Assert.NotNull(first);

        // After first update the token must advance
        await repo.UpsertSnapshotAsync(first with { CurrentState = "Queued" });
        var second = await repo.GetSnapshotAsync(orderId);
        Assert.NotNull(second);
        Assert.True(second.RowVersion > first.RowVersion,
            "RowVersion must advance after each successful write.");
    }

    [Fact]
    public async Task SuccessiveUpdates_EachWithCurrentRowVersion_AllSucceed()
    {
        await using var ctx = BuildContext();
        var repo    = new StoreOrderRepository(ctx);
        var orderId = Guid.NewGuid();

        await repo.UpsertSnapshotAsync(MakeSnapshot(orderId, "Received", rowVersion: 0));

        string[] transitions = ["Queued", "InProgress", "Ready", "Completed"];
        foreach (var state in transitions)
        {
            var current = await repo.GetSnapshotAsync(orderId);
            Assert.NotNull(current);
            await repo.UpsertSnapshotAsync(current with { CurrentState = state });
        }

        var final = await repo.GetSnapshotAsync(orderId);
        Assert.NotNull(final);
        Assert.Equal("Completed", final.CurrentState);
    }

    [Fact]
    public async Task ConcurrencyConflict_DoesNotCorruptExistingSnapshot()
    {
        await using var ctx = BuildContext();
        var repo    = new StoreOrderRepository(ctx);
        var orderId = Guid.NewGuid();

        await repo.UpsertSnapshotAsync(MakeSnapshot(orderId, "Received", rowVersion: 0));

        var readerA = await repo.GetSnapshotAsync(orderId);
        Assert.NotNull(readerA);

        // A succeeds
        await repo.UpsertSnapshotAsync(readerA with { CurrentState = "Queued" });

        // B fails with stale token
        _ = await Assert.ThrowsAsync<DbUpdateConcurrencyException>(
            () => repo.UpsertSnapshotAsync(readerA with { CurrentState = "Cancelled" }));

        // State in DB must still be the one committed by A
        var stored = await repo.GetSnapshotAsync(orderId);
        Assert.NotNull(stored);
        Assert.Equal("Queued", stored.CurrentState);
    }
}

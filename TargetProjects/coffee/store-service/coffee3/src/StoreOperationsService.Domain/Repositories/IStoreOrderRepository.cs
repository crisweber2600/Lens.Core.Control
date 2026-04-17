namespace StoreOperationsService.Domain.Repositories;

/// <summary>
/// Snapshot data transferred between the domain and the persistence layer.
/// </summary>
public sealed record StoreOrderSnapshotData(
    Guid OrderId,
    string CurrentState,
    string PriorityBand,
    bool IsRush,
    bool IsAtRisk,
    DateTimeOffset CreatedAt,
    DateTimeOffset UpdatedAt);

/// <summary>
/// Persistence contract for the StoreOrder aggregate snapshot.
/// </summary>
public interface IStoreOrderRepository
{
    /// <summary>
    /// Inserts or updates the snapshot row for the given order.
    /// Uses OrderId as the natural key.
    /// </summary>
    Task UpsertSnapshotAsync(StoreOrderSnapshotData snapshot, CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns the current snapshot for an order, or null if not found.
    /// </summary>
    Task<StoreOrderSnapshotData?> GetSnapshotAsync(Guid orderId, CancellationToken cancellationToken = default);
}

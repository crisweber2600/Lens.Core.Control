namespace StoreOperationsService.Domain.Repositories;

/// <summary>
/// Snapshot data transferred between the domain and the persistence layer.
/// <para>
/// <see cref="RowVersion"/> is the optimistic-concurrency token read from the last successful
/// <see cref="IStoreOrderRepository.GetSnapshotAsync"/> call.  Pass the same value back through
/// <see cref="IStoreOrderRepository.UpsertSnapshotAsync"/> to guard against dirty writes.
/// New snapshots (inserts) should leave <see cref="RowVersion"/> at its default value of 0.
/// </para>
/// </summary>
public sealed record StoreOrderSnapshotData(
    Guid OrderId,
    string CurrentState,
    string PriorityBand,
    bool IsRush,
    bool IsAtRisk,
    DateTimeOffset CreatedAt,
    DateTimeOffset UpdatedAt,
    uint RowVersion = 0);

/// <summary>
/// Persistence contract for the StoreOrder aggregate snapshot and transition log.
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

    /// <summary>
    /// Appends a transition log entry for the given order.
    /// </summary>
    Task AppendTransitionLogAsync(Guid orderId, string fromState, string toState, DateTimeOffset occurredAt, CancellationToken cancellationToken = default);
}


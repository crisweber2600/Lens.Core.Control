namespace StoreOperationsService.Infrastructure.Entities;

/// <summary>
/// Persisted snapshot of the current state of a StoreOrder aggregate.
/// One row per order; updated on every lifecycle transition.
/// </summary>
public sealed class StoreOrderSnapshot
{
    public Guid OrderId { get; set; }
    public string CurrentState { get; set; } = string.Empty;
    public string PriorityBand { get; set; } = string.Empty;
    public bool IsRush { get; set; }
    public bool IsAtRisk { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
    public uint RowVersion { get; set; }

    /// <summary>Persisted <see cref="Domain.ValueObjects.OrderType"/> name (e.g. "Drink").</summary>
    public string OrderType { get; set; } = "Unknown";

    /// <summary>The instant the order entered the Queued state. Null until queued.</summary>
    public DateTimeOffset? QueuedAt { get; set; }
}

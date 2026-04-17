namespace StoreOperationsService.Infrastructure.Entities;

/// <summary>
/// Immutable append-only log of every lifecycle transition for a StoreOrder.
/// </summary>
public sealed class OrderTransitionLog
{
    public long Id { get; set; }
    public Guid OrderId { get; set; }
    public string FromState { get; set; } = string.Empty;
    public string ToState { get; set; } = string.Empty;
    public DateTimeOffset OccurredAt { get; set; }
}

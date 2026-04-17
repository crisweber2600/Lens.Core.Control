namespace StoreOperationsService.Domain.Events;

/// <summary>
/// Marker interface for all domain events emitted by StoreOperationsService aggregates.
/// </summary>
public interface IDomainEvent
{
    Guid EventId { get; }
    Guid OrderId { get; }
    DateTimeOffset OccurredAt { get; }
    int SchemaVersion { get; }
}

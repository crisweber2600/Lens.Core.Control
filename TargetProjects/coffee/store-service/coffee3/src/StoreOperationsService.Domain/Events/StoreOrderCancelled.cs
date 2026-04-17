namespace StoreOperationsService.Domain.Events;

/// <summary>
/// Domain event raised when a store order is cancelled from any non-terminal state.
/// Published atomically with the snapshot update and transition log entry via the outbox.
/// </summary>
public sealed record StoreOrderCancelled(
    Guid EventId,
    Guid OrderId,
    Guid CustomerOrderId,
    Guid StoreId,
    DateTimeOffset CancelledAt,
    string CancelledBy,
    string ReasonCode,
    string CancellationStage,
    long AggregateVersion,
    DateTimeOffset OccurredAt,
    string? CorrelationId = null,
    int SchemaVersion = 1) : IDomainEvent;

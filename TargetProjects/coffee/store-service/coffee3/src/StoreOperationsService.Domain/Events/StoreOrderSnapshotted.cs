namespace StoreOperationsService.Domain.Events;

/// <summary>
/// Emitted on every accepted lifecycle transition or modifier change.
/// Contains the full current-state snapshot per AD-5 (versioned snapshot events
/// are the canonical downstream contract — no delta events).
///
/// Schema version starts at 1. Increment when adding non-optional fields.
/// </summary>
/// <param name="EventId">Unique identifier for this event occurrence.</param>
/// <param name="OrderId">The store order that transitioned.</param>
/// <param name="StoreId">The store that owns this order.</param>
/// <param name="CustomerId">The customer associated with this order.</param>
/// <param name="PreviousState">Lifecycle state before the transition.</param>
/// <param name="CurrentState">Lifecycle state after the transition.</param>
/// <param name="IsRush">Whether the order carries rush priority at the time of this event.</param>
/// <param name="IsAtRisk">Whether the order is flagged as at-risk at the time of this event.</param>
/// <param name="PriorityBand">The priority band applied at the time of this event.</param>
/// <param name="AggregateVersion">Monotonic aggregate version number for ordering guarantees.</param>
/// <param name="OccurredAt">Timestamp when the transition was accepted.</param>
/// <param name="CorrelationId">Optional trace/correlation identifier from the originating command.</param>
/// <param name="SchemaVersion">Always 1 for this contract version.</param>
public sealed record StoreOrderSnapshotted(
    Guid EventId,
    Guid OrderId,
    Guid StoreId,
    Guid CustomerId,
    string PreviousState,
    string CurrentState,
    bool IsRush,
    bool IsAtRisk,
    string PriorityBand,
    long AggregateVersion,
    DateTimeOffset OccurredAt,
    string? CorrelationId = null,
    int SchemaVersion = 1) : IDomainEvent;

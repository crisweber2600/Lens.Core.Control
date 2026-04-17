namespace StoreOperationsService.Domain.Events;

/// <summary>
/// Emitted when an order transitions from <c>Ready</c> to <c>Completed</c>.
/// Published to the outbox so CustomerService can close its confirmation workflow
/// (per decisions.md Q2 — SB4 resolution).
///
/// Fields match the canonical schema defined in decisions.md:
///   store_order_id, customer_order_id, store_id, completed_at, completed_by,
///   completion_mode, aggregate_version, correlation_id
///
/// Schema version starts at 1.
/// </summary>
/// <param name="EventId">Unique identifier for this event occurrence.</param>
/// <param name="OrderId">Store-side order identifier (maps to <c>store_order_id</c>).</param>
/// <param name="CustomerOrderId">Upstream customer order identifier.</param>
/// <param name="StoreId">The store that completed the order.</param>
/// <param name="CompletedAt">Timestamp when completion was accepted.</param>
/// <param name="CompletedBy">Actor identity (operator or system) that confirmed completion.</param>
/// <param name="CompletionMode">Describes how completion was confirmed (e.g. <c>handoff-confirmed</c>).</param>
/// <param name="AggregateVersion">Monotonic aggregate version for ordering guarantees.</param>
/// <param name="OccurredAt">Alias of <paramref name="CompletedAt"/>; satisfies <see cref="IDomainEvent"/> contract.</param>
/// <param name="CorrelationId">Optional trace/correlation identifier from the originating command.</param>
/// <param name="SchemaVersion">Always 1 for this contract version.</param>
public sealed record StoreOrderCompleted(
    Guid EventId,
    Guid OrderId,
    Guid CustomerOrderId,
    Guid StoreId,
    DateTimeOffset CompletedAt,
    string CompletedBy,
    string CompletionMode,
    long AggregateVersion,
    DateTimeOffset OccurredAt,
    string? CorrelationId = null,
    int SchemaVersion = 1) : IDomainEvent;

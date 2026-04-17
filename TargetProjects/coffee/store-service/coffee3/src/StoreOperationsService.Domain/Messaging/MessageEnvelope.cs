namespace StoreOperationsService.Domain.Messaging;

/// <summary>
/// Carrier for a single outbound event message dispatched through the event bus adapter.
/// </summary>
public sealed record MessageEnvelope(
    string MessageId,
    string EventType,
    string Payload,
    DateTimeOffset OccurredAt,
    string? CorrelationId = null
);

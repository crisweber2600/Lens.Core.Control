namespace StoreOperationsService.Infrastructure.Entities;

/// <summary>
/// Represents an outbound domain event queued for publication.
/// Written in the same transaction as the aggregate state change.
/// The Outbox Publisher background service reads unsent rows and delivers them.
/// </summary>
public sealed class OutboxMessage
{
    /// <summary>Identity key (auto-incremented).</summary>
    public long Id { get; set; }

    /// <summary>Fully-qualified CLR type name of the domain event payload.</summary>
    public string Type { get; set; } = string.Empty;

    /// <summary>JSON-serialised domain event.</summary>
    public string Payload { get; set; } = string.Empty;

    /// <summary>When the event was enqueued (UTC).</summary>
    public DateTimeOffset OccurredAt { get; set; }

    /// <summary>When the message was successfully published; null if not yet sent.</summary>
    public DateTimeOffset? SentAt { get; set; }

    /// <summary>Optional correlation id propagated from the originating command.</summary>
    public string? CorrelationId { get; set; }
}

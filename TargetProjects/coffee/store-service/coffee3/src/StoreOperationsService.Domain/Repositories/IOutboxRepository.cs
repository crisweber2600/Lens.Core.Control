namespace StoreOperationsService.Domain.Repositories;

/// <summary>
/// Persistence contract for the transactional outbox.
/// Implementations must write messages in the same transaction as aggregate state changes.
/// </summary>
public interface IOutboxRepository
{
    /// <summary>
    /// Enqueues a serialised domain event for outbound publication.
    /// </summary>
    /// <param name="type">Fully-qualified CLR type name of the domain event.</param>
    /// <param name="payload">JSON-serialised event payload.</param>
    /// <param name="occurredAt">Timestamp when the event was raised.</param>
    /// <param name="correlationId">Optional correlation id from the originating command.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task EnqueueAsync(
        string type,
        string payload,
        DateTimeOffset occurredAt,
        string? correlationId = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns all unsent messages ordered by <see cref="occurredAt"/> ascending.
    /// </summary>
    Task<IReadOnlyList<OutboxMessageDto>> GetUnsentAsync(
        int batchSize,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Marks a message as successfully published.
    /// </summary>
    Task MarkSentAsync(long id, DateTimeOffset sentAt, CancellationToken cancellationToken = default);
}

/// <summary>Read-model returned by <see cref="IOutboxRepository.GetUnsentAsync"/>.</summary>
public sealed record OutboxMessageDto(
    long Id,
    string Type,
    string Payload,
    DateTimeOffset OccurredAt,
    string? CorrelationId);

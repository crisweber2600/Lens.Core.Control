using Microsoft.EntityFrameworkCore;
using StoreOperationsService.Domain.Repositories;
using StoreOperationsService.Infrastructure.Entities;

namespace StoreOperationsService.Infrastructure.Repositories;

/// <summary>
/// EF Core implementation of <see cref="IOutboxRepository"/>.
/// Reads and marks outbox messages for the Outbox Publisher background service.
/// </summary>
public sealed class OutboxRepository(StoreOperationsDbContext dbContext) : IOutboxRepository
{
    public async Task EnqueueAsync(
        string type,
        string payload,
        DateTimeOffset occurredAt,
        string? correlationId = null,
        CancellationToken cancellationToken = default)
    {
        dbContext.OutboxMessages.Add(new OutboxMessage
        {
            Type          = type,
            Payload       = payload,
            OccurredAt    = occurredAt,
            CorrelationId = correlationId,
        });
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<OutboxMessageDto>> GetUnsentAsync(
        int batchSize,
        CancellationToken cancellationToken = default)
    {
        var messages = await dbContext.OutboxMessages
            .Where(m => m.SentAt == null)
            .OrderBy(m => m.OccurredAt)
            .Take(batchSize)
            .ToListAsync(cancellationToken);

        return messages
            .Select(m => new OutboxMessageDto(m.Id, m.Type, m.Payload, m.OccurredAt, m.CorrelationId))
            .ToList()
            .AsReadOnly();
    }

    public async Task MarkSentAsync(
        long id,
        DateTimeOffset sentAt,
        CancellationToken cancellationToken = default)
    {
        var message = await dbContext.OutboxMessages.FindAsync([id], cancellationToken);
        if (message is not null)
        {
            message.SentAt = sentAt;
            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}

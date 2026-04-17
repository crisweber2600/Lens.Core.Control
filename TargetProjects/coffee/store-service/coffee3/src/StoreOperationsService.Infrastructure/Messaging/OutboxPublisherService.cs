using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using StoreOperationsService.Domain.Messaging;
using StoreOperationsService.Domain.Repositories;

namespace StoreOperationsService.Infrastructure.Messaging;

/// <summary>
/// Hosted background service that polls the outbox table and publishes
/// any unsent messages to the event bus, then marks them as sent.
/// </summary>
public sealed class OutboxPublisherService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IEventBusAdapter _eventBus;
    private readonly ILogger<OutboxPublisherService> _logger;
    private readonly TimeSpan _pollInterval;

    /// <summary>Number of messages fetched per poll cycle.</summary>
    public const int BatchSize = 50;

    public OutboxPublisherService(
        IServiceScopeFactory scopeFactory,
        IEventBusAdapter eventBus,
        ILogger<OutboxPublisherService> logger,
        TimeSpan? pollInterval = null)
    {
        _scopeFactory = scopeFactory;
        _eventBus     = eventBus;
        _logger       = logger;
        _pollInterval = pollInterval ?? TimeSpan.FromSeconds(5);
    }

    /// <inheritdoc/>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Outbox Publisher started.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await PublishBatchAsync(stoppingToken);
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
            {
                break;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error in outbox publisher loop.");
            }

            try
            {
                await Task.Delay(_pollInterval, stoppingToken);
            }
            catch (OperationCanceledException)
            {
                break;
            }
        }

        _logger.LogInformation("Outbox Publisher stopped.");
    }

    /// <summary>
    /// Fetches one batch of unsent messages and publishes each.
    /// Per-message failures are logged and skipped so the batch continues.
    /// </summary>
    public async Task PublishBatchAsync(CancellationToken cancellationToken)
    {
        using var scope = _scopeFactory.CreateScope();
        var outboxRepo = scope.ServiceProvider.GetRequiredService<IOutboxRepository>();

        var messages = await outboxRepo.GetUnsentAsync(BatchSize, cancellationToken);

        foreach (var message in messages)
        {
            try
            {
                var envelope = new MessageEnvelope(
                    MessageId:     message.Id.ToString(),
                    EventType:     message.Type,
                    Payload:       message.Payload,
                    OccurredAt:    message.OccurredAt,
                    CorrelationId: message.CorrelationId);

                await _eventBus.PublishAsync(envelope, cancellationToken);
                await outboxRepo.MarkSentAsync(message.Id, DateTimeOffset.UtcNow, cancellationToken);

                _logger.LogDebug(
                    "Published outbox message {Id} of type {Type}.",
                    message.Id, message.Type);
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Failed to publish outbox message {Id} of type {Type}. Skipping.",
                    message.Id, message.Type);
            }
        }
    }
}

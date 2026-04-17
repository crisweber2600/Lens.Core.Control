using StoreOperationsService.Domain.Messaging;

namespace StoreOperationsService.Infrastructure.Messaging;

/// <summary>
/// In-process event bus adapter for local development and testing.
/// Stores published envelopes in memory for inspection; does not connect to
/// any external broker.
/// </summary>
public sealed class InMemoryEventBusAdapter : IEventBusAdapter
{
    private readonly List<MessageEnvelope> _published = new();
    private readonly object _lock = new();

    /// <summary>
    /// Returns a snapshot of all envelopes published during the lifetime of this instance.
    /// </summary>
    public IReadOnlyList<MessageEnvelope> PublishedMessages
    {
        get { lock (_lock) { return _published.ToList().AsReadOnly(); } }
    }

    /// <inheritdoc />
    public Task PublishAsync(MessageEnvelope envelope, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        lock (_lock)
        {
            _published.Add(envelope);
        }
        return Task.CompletedTask;
    }
}

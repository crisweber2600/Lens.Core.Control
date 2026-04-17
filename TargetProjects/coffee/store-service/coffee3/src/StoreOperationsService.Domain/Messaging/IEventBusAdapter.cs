namespace StoreOperationsService.Domain.Messaging;

/// <summary>
/// Abstraction over the message broker. Decouples the Outbox Publisher from
/// any specific transport (RabbitMQ, Azure Service Bus, in-memory stub, etc.).
/// </summary>
public interface IEventBusAdapter
{
    /// <summary>
    /// Publishes a single message envelope to the underlying message broker.
    /// </summary>
    Task PublishAsync(MessageEnvelope envelope, CancellationToken cancellationToken = default);
}

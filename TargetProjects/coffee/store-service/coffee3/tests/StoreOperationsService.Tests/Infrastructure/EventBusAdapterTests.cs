using StoreOperationsService.Domain.Messaging;
using StoreOperationsService.Infrastructure.Messaging;

namespace StoreOperationsService.Tests.Infrastructure;

public sealed class EventBusAdapterTests
{
    private static MessageEnvelope MakeEnvelope(
        string eventType = "StoreOrderReceived",
        string? correlationId = null) =>
        new(
            MessageId: Guid.NewGuid().ToString(),
            EventType: eventType,
            Payload: "{}",
            OccurredAt: DateTimeOffset.UtcNow,
            CorrelationId: correlationId
        );

    [Fact]
    public void PublishedMessages_IsEmpty_BeforeAnyPublish()
    {
        var adapter = new InMemoryEventBusAdapter();

        Assert.Empty(adapter.PublishedMessages);
    }

    [Fact]
    public async Task PublishAsync_AddsEnvelope_ToPublishedMessages()
    {
        var adapter = new InMemoryEventBusAdapter();
        var envelope = MakeEnvelope();

        await adapter.PublishAsync(envelope);

        Assert.Single(adapter.PublishedMessages);
        Assert.Equal(envelope.MessageId, adapter.PublishedMessages[0].MessageId);
    }

    [Fact]
    public async Task PublishAsync_MultipleEnvelopes_AllAccumulate()
    {
        var adapter = new InMemoryEventBusAdapter();
        var e1 = MakeEnvelope("StoreOrderReceived");
        var e2 = MakeEnvelope("StoreOrderQueued");
        var e3 = MakeEnvelope("StoreOrderInProgress");

        await adapter.PublishAsync(e1);
        await adapter.PublishAsync(e2);
        await adapter.PublishAsync(e3);

        Assert.Equal(3, adapter.PublishedMessages.Count);
        Assert.Equal(e1.MessageId, adapter.PublishedMessages[0].MessageId);
        Assert.Equal(e2.MessageId, adapter.PublishedMessages[1].MessageId);
        Assert.Equal(e3.MessageId, adapter.PublishedMessages[2].MessageId);
    }

    [Fact]
    public async Task PublishAsync_PreservesAllEnvelopeFields()
    {
        var adapter = new InMemoryEventBusAdapter();
        var now = DateTimeOffset.UtcNow;
        var envelope = new MessageEnvelope(
            MessageId: "msg-001",
            EventType: "StoreOrderCompleted",
            Payload: """{"orderId":"abc"}""",
            OccurredAt: now,
            CorrelationId: "corr-xyz"
        );

        await adapter.PublishAsync(envelope);

        var stored = adapter.PublishedMessages[0];
        Assert.Equal("msg-001", stored.MessageId);
        Assert.Equal("StoreOrderCompleted", stored.EventType);
        Assert.Equal("""{"orderId":"abc"}""", stored.Payload);
        Assert.Equal(now, stored.OccurredAt);
        Assert.Equal("corr-xyz", stored.CorrelationId);
    }

    [Fact]
    public async Task PublishAsync_WithNullCorrelationId_Succeeds()
    {
        var adapter = new InMemoryEventBusAdapter();
        var envelope = MakeEnvelope(correlationId: null);

        await adapter.PublishAsync(envelope);

        Assert.Single(adapter.PublishedMessages);
        Assert.Null(adapter.PublishedMessages[0].CorrelationId);
    }

    [Fact]
    public async Task PublishAsync_WithCancelledToken_ThrowsOperationCanceledException()
    {
        var adapter = new InMemoryEventBusAdapter();
        var envelope = MakeEnvelope();
        using var cts = new CancellationTokenSource();
        cts.Cancel();

        await Assert.ThrowsAnyAsync<OperationCanceledException>(
            () => adapter.PublishAsync(envelope, cts.Token));
    }

    [Fact]
    public async Task PublishAsync_AfterCancellation_DoesNotStoreEnvelope()
    {
        var adapter = new InMemoryEventBusAdapter();
        var envelope = MakeEnvelope();
        using var cts = new CancellationTokenSource();
        cts.Cancel();

        try { await adapter.PublishAsync(envelope, cts.Token); } catch { }

        Assert.Empty(adapter.PublishedMessages);
    }

    [Fact]
    public void InMemoryEventBusAdapter_ImplementsIEventBusAdapter()
    {
        var adapter = new InMemoryEventBusAdapter();
        Assert.IsAssignableFrom<IEventBusAdapter>(adapter);
    }

    [Fact]
    public async Task PublishedMessages_Snapshot_IsIsolatedFromFuturePublishes()
    {
        var adapter = new InMemoryEventBusAdapter();
        await adapter.PublishAsync(MakeEnvelope());

        var snapshot = adapter.PublishedMessages;

        await adapter.PublishAsync(MakeEnvelope());

        // snapshot captured before second publish should still contain 1
        Assert.Equal(1, snapshot.Count);
        Assert.Equal(2, adapter.PublishedMessages.Count);
    }
}

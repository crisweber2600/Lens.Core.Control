using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;
using StoreOperationsService.Domain.Messaging;
using StoreOperationsService.Domain.Repositories;
using StoreOperationsService.Infrastructure.Messaging;

namespace StoreOperationsService.Tests.Infrastructure;

/// <summary>
/// Unit tests for <see cref="OutboxPublisherService"/>.
/// Uses in-process fakes so no external infrastructure is required.
/// </summary>
public sealed class OutboxPublisherServiceTests
{
    // ─────────────────────────────────────────────────────────────────────────
    // Fakes
    // ─────────────────────────────────────────────────────────────────────────

    /// <summary>Configurable in-memory IOutboxRepository stub.</summary>
    private sealed class FakeOutboxRepository : IOutboxRepository
    {
        private readonly List<OutboxMessageDto> _unsent;
        private readonly Dictionary<long, DateTimeOffset> _sentAt = new();

        public FakeOutboxRepository(IEnumerable<OutboxMessageDto>? messages = null)
        {
            _unsent = messages?.ToList() ?? new List<OutboxMessageDto>();
        }

        public IReadOnlyDictionary<long, DateTimeOffset> SentAt => _sentAt;

        public Task EnqueueAsync(
            string type, string payload, DateTimeOffset occurredAt,
            string? correlationId = null, CancellationToken cancellationToken = default)
        {
            var id = _unsent.Count > 0 ? _unsent.Max(m => m.Id) + 1 : 1L;
            _unsent.Add(new OutboxMessageDto(id, type, payload, occurredAt, correlationId));
            return Task.CompletedTask;
        }

        public Task<IReadOnlyList<OutboxMessageDto>> GetUnsentAsync(
            int batchSize, CancellationToken cancellationToken = default)
        {
            var result = _unsent
                .Where(m => !_sentAt.ContainsKey(m.Id))
                .Take(batchSize)
                .ToList();
            return Task.FromResult<IReadOnlyList<OutboxMessageDto>>(result.AsReadOnly());
        }

        public Task MarkSentAsync(long id, DateTimeOffset sentAt, CancellationToken cancellationToken = default)
        {
            _sentAt[id] = sentAt;
            return Task.CompletedTask;
        }
    }

    /// <summary>Event bus adapter that throws on every PublishAsync call.</summary>
    private sealed class FailingEventBusAdapter : IEventBusAdapter
    {
        public int CallCount { get; private set; }

        public Task PublishAsync(MessageEnvelope envelope, CancellationToken cancellationToken = default)
        {
            CallCount++;
            throw new InvalidOperationException("Simulated broker failure.");
        }
    }

    // ─────────────────────────────────────────────────────────────────────────
    // Helpers
    // ─────────────────────────────────────────────────────────────────────────

    private static IServiceScopeFactory MakeScopeFactory(IOutboxRepository repo)
    {
        var services = new ServiceCollection();
        services.AddScoped<IOutboxRepository>(_ => repo);
        var provider = services.BuildServiceProvider();
        return provider.GetRequiredService<IServiceScopeFactory>();
    }

    private static OutboxPublisherService MakeService(
        IOutboxRepository repo,
        IEventBusAdapter? bus = null)
    {
        return new OutboxPublisherService(
            MakeScopeFactory(repo),
            bus ?? new InMemoryEventBusAdapter(),
            NullLogger<OutboxPublisherService>.Instance,
            pollInterval: TimeSpan.FromMilliseconds(10));
    }

    private static OutboxMessageDto MakeDto(
        long id = 1,
        string type = "StoreOrderCompleted",
        string payload = "{}",
        string? correlationId = null)
        => new(id, type, payload, DateTimeOffset.UtcNow, correlationId);

    // ─────────────────────────────────────────────────────────────────────────
    // Tests
    // ─────────────────────────────────────────────────────────────────────────

    [Fact]
    public async Task PublishBatchAsync_EmptyOutbox_PublishesNothing()
    {
        var repo = new FakeOutboxRepository();
        var bus  = new InMemoryEventBusAdapter();
        var svc  = MakeService(repo, bus);

        await svc.PublishBatchAsync(CancellationToken.None);

        Assert.Empty(bus.PublishedMessages);
    }

    [Fact]
    public async Task PublishBatchAsync_OneUnsentMessage_PublishesEnvelope()
    {
        var dto  = MakeDto(id: 42, type: "StoreOrderCompleted", payload: """{"orderId":"abc"}""");
        var repo = new FakeOutboxRepository(new[] { dto });
        var bus  = new InMemoryEventBusAdapter();
        var svc  = MakeService(repo, bus);

        await svc.PublishBatchAsync(CancellationToken.None);

        Assert.Single(bus.PublishedMessages);
        var msg = bus.PublishedMessages[0];
        Assert.Equal("42", msg.MessageId);
        Assert.Equal("StoreOrderCompleted", msg.EventType);
        Assert.Equal("""{"orderId":"abc"}""", msg.Payload);
    }

    [Fact]
    public async Task PublishBatchAsync_OneUnsentMessage_MarksAsSent()
    {
        var dto  = MakeDto(id: 7);
        var repo = new FakeOutboxRepository(new[] { dto });
        var svc  = MakeService(repo);

        await svc.PublishBatchAsync(CancellationToken.None);

        Assert.True(repo.SentAt.ContainsKey(7L));
        Assert.NotEqual(default, repo.SentAt[7L]);
    }

    [Fact]
    public async Task PublishBatchAsync_MultipleMessages_PublishesAll()
    {
        var messages = new[]
        {
            MakeDto(id: 1, type: "StoreOrderReceived"),
            MakeDto(id: 2, type: "StoreOrderQueued"),
            MakeDto(id: 3, type: "StoreOrderCompleted"),
        };
        var repo = new FakeOutboxRepository(messages);
        var bus  = new InMemoryEventBusAdapter();
        var svc  = MakeService(repo, bus);

        await svc.PublishBatchAsync(CancellationToken.None);

        Assert.Equal(3, bus.PublishedMessages.Count);
        Assert.Equal("1", bus.PublishedMessages[0].MessageId);
        Assert.Equal("2", bus.PublishedMessages[1].MessageId);
        Assert.Equal("3", bus.PublishedMessages[2].MessageId);
    }

    [Fact]
    public async Task PublishBatchAsync_MultipleMessages_MarksAllSent()
    {
        var messages = new[]
        {
            MakeDto(id: 10),
            MakeDto(id: 20),
        };
        var repo = new FakeOutboxRepository(messages);
        var svc  = MakeService(repo);

        await svc.PublishBatchAsync(CancellationToken.None);

        Assert.True(repo.SentAt.ContainsKey(10L));
        Assert.True(repo.SentAt.ContainsKey(20L));
    }

    [Fact]
    public async Task PublishBatchAsync_AlreadySentMessages_NotRePublished()
    {
        // Simulate message 1 already sent (SentAt populated) and message 2 unsent.
        var msg1 = MakeDto(id: 1);
        var msg2 = MakeDto(id: 2);
        var repo = new FakeOutboxRepository(new[] { msg1, msg2 });

        // Pre-mark msg1 as sent so GetUnsentAsync won't return it.
        await repo.MarkSentAsync(1L, DateTimeOffset.UtcNow);

        var bus = new InMemoryEventBusAdapter();
        var svc = MakeService(repo, bus);

        await svc.PublishBatchAsync(CancellationToken.None);

        Assert.Single(bus.PublishedMessages);
        Assert.Equal("2", bus.PublishedMessages[0].MessageId);
    }

    [Fact]
    public async Task PublishBatchAsync_PublishFails_MessageNotMarkedSent()
    {
        var dto  = MakeDto(id: 99);
        var repo = new FakeOutboxRepository(new[] { dto });
        var bus  = new FailingEventBusAdapter();
        var svc  = MakeService(repo, bus);

        // Should not throw — error is swallowed per message.
        await svc.PublishBatchAsync(CancellationToken.None);

        Assert.False(repo.SentAt.ContainsKey(99L));
    }

    [Fact]
    public async Task PublishBatchAsync_FirstMessageFails_SecondMessagePublished()
    {
        // A bus that fails on the first call, succeeds on subsequent calls.
        var msg1 = MakeDto(id: 1, type: "Failing");
        var msg2 = MakeDto(id: 2, type: "Succeeding");
        var repo = new FakeOutboxRepository(new[] { msg1, msg2 });

        var recordingBus = new InMemoryEventBusAdapter();
        int callCount    = 0;
        var bus          = new PartiallyFailingBus(failOnCalls: new HashSet<int> { 1 }, inner: recordingBus, ref callCount);

        var svc = new OutboxPublisherService(
            MakeScopeFactory(repo),
            bus,
            NullLogger<OutboxPublisherService>.Instance,
            pollInterval: TimeSpan.FromMilliseconds(10));

        await svc.PublishBatchAsync(CancellationToken.None);

        // msg2 should still be published.
        Assert.Single(recordingBus.PublishedMessages);
        Assert.Equal("2", recordingBus.PublishedMessages[0].MessageId);
        Assert.False(repo.SentAt.ContainsKey(1L));
        Assert.True(repo.SentAt.ContainsKey(2L));
    }

    [Fact]
    public async Task PublishBatchAsync_CorrelationId_IsPreservedInEnvelope()
    {
        var dto  = MakeDto(id: 5, correlationId: "corr-abc-123");
        var repo = new FakeOutboxRepository(new[] { dto });
        var bus  = new InMemoryEventBusAdapter();
        var svc  = MakeService(repo, bus);

        await svc.PublishBatchAsync(CancellationToken.None);

        Assert.Equal("corr-abc-123", bus.PublishedMessages[0].CorrelationId);
    }

    [Fact]
    public async Task PublishBatchAsync_NullCorrelationId_EnvelopeCorrelationIdIsNull()
    {
        var dto  = MakeDto(id: 6, correlationId: null);
        var repo = new FakeOutboxRepository(new[] { dto });
        var bus  = new InMemoryEventBusAdapter();
        var svc  = MakeService(repo, bus);

        await svc.PublishBatchAsync(CancellationToken.None);

        Assert.Null(bus.PublishedMessages[0].CorrelationId);
    }

    [Fact]
    public async Task PublishBatchAsync_EnvelopeOccurredAt_MatchesOutboxOccurredAt()
    {
        var now = new DateTimeOffset(2025, 6, 1, 12, 0, 0, TimeSpan.Zero);
        var dto  = new OutboxMessageDto(1L, "StoreOrderReady", "{}", now, null);
        var repo = new FakeOutboxRepository(new[] { dto });
        var bus  = new InMemoryEventBusAdapter();
        var svc  = MakeService(repo, bus);

        await svc.PublishBatchAsync(CancellationToken.None);

        Assert.Equal(now, bus.PublishedMessages[0].OccurredAt);
    }

    [Fact]
    public async Task ExecuteAsync_StopsOnCancellation()
    {
        var repo = new FakeOutboxRepository();
        var bus  = new InMemoryEventBusAdapter();
        var svc  = MakeService(repo, bus);

        using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(50));

        // Should complete without throwing.
        await svc.StartAsync(cts.Token);
        await Task.Delay(100);
        await svc.StopAsync(CancellationToken.None);
    }

    [Fact]
    public async Task PublishBatchAsync_BatchSizeRespected()
    {
        // Insert 60 messages — only BatchSize (50) should be fetched per call.
        var messages = Enumerable.Range(1, 60)
            .Select(i => MakeDto(id: i))
            .ToArray();
        var repo = new FakeOutboxRepository(messages);
        var bus  = new InMemoryEventBusAdapter();
        var svc  = MakeService(repo, bus);

        await svc.PublishBatchAsync(CancellationToken.None);

        Assert.Equal(OutboxPublisherService.BatchSize, bus.PublishedMessages.Count);
    }

    // ─────────────────────────────────────────────────────────────────────────
    // Helper bus that fails on specific call ordinals
    // ─────────────────────────────────────────────────────────────────────────

    private sealed class PartiallyFailingBus : IEventBusAdapter
    {
        private readonly HashSet<int> _failOnCalls;
        private readonly InMemoryEventBusAdapter _inner;
        private int _count;

        public PartiallyFailingBus(HashSet<int> failOnCalls, InMemoryEventBusAdapter inner, ref int count)
        {
            _failOnCalls = failOnCalls;
            _inner       = inner;
            _count       = count;
        }

        public async Task PublishAsync(MessageEnvelope envelope, CancellationToken cancellationToken = default)
        {
            _count++;
            if (_failOnCalls.Contains(_count))
                throw new InvalidOperationException($"Simulated failure on call {_count}.");

            await _inner.PublishAsync(envelope, cancellationToken);
        }
    }
}

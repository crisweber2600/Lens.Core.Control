using System.Text.Json;
using StoreOperationsService.Domain.Events;

namespace StoreOperationsService.Tests.Domain;

public sealed class StoreOrderSnapshottedTests
{
    // ── Schema version ─────────────────────────────────────────────────────

    [Fact]
    public void SchemaVersion_DefaultsToOne()
    {
        var evt = BuildSnapshot();
        Assert.Equal(1, evt.SchemaVersion);
    }

    [Fact]
    public void SchemaVersion_IsNonZero()
    {
        var evt = BuildSnapshot();
        Assert.True(evt.SchemaVersion > 0, "SchemaVersion must be positive");
    }

    // ── Required fields populated ──────────────────────────────────────────

    [Fact]
    public void EventId_IsNotEmpty()
    {
        var evt = BuildSnapshot();
        Assert.NotEqual(Guid.Empty, evt.EventId);
    }

    [Fact]
    public void OrderId_IsNotEmpty()
    {
        var evt = BuildSnapshot();
        Assert.NotEqual(Guid.Empty, evt.OrderId);
    }

    [Fact]
    public void StoreId_IsNotEmpty()
    {
        var evt = BuildSnapshot();
        Assert.NotEqual(Guid.Empty, evt.StoreId);
    }

    [Fact]
    public void CurrentState_IsNotNullOrEmpty()
    {
        var evt = BuildSnapshot();
        Assert.False(string.IsNullOrEmpty(evt.CurrentState));
    }

    [Fact]
    public void PreviousState_IsNotNullOrEmpty()
    {
        var evt = BuildSnapshot();
        Assert.False(string.IsNullOrEmpty(evt.PreviousState));
    }

    [Fact]
    public void OccurredAt_IsNotDefault()
    {
        var evt = BuildSnapshot();
        Assert.NotEqual(default, evt.OccurredAt);
    }

    [Fact]
    public void AggregateVersion_IsPositive()
    {
        var evt = BuildSnapshot();
        Assert.True(evt.AggregateVersion > 0);
    }

    // ── IDomainEvent contract ──────────────────────────────────────────────

    [Fact]
    public void ImplementsIDomainEvent()
    {
        IDomainEvent evt = BuildSnapshot();
        Assert.Equal(evt.OrderId, evt.OrderId);
        Assert.Equal(1, evt.SchemaVersion);
    }

    // ── JSON serializability ───────────────────────────────────────────────

    [Fact]
    public void IsJsonSerializable_RoundTrip()
    {
        var evt = BuildSnapshot();
        var json = JsonSerializer.Serialize(evt);
        var deserialized = JsonSerializer.Deserialize<StoreOrderSnapshotted>(json);
        Assert.NotNull(deserialized);
        Assert.Equal(evt.EventId, deserialized!.EventId);
        Assert.Equal(evt.OrderId, deserialized.OrderId);
        Assert.Equal(evt.CurrentState, deserialized.CurrentState);
        Assert.Equal(evt.SchemaVersion, deserialized.SchemaVersion);
    }

    [Fact]
    public void Json_ContainsSchemaVersionField()
    {
        var evt = BuildSnapshot();
        var json = JsonSerializer.Serialize(evt);
        Assert.Contains("SchemaVersion", json);
    }

    [Fact]
    public void Json_ContainsOrderIdField()
    {
        var evt = BuildSnapshot();
        var json = JsonSerializer.Serialize(evt);
        Assert.Contains("OrderId", json);
    }

    // ── Optional correlation id ────────────────────────────────────────────

    [Fact]
    public void CorrelationId_DefaultsToNull()
    {
        var evt = BuildSnapshot();
        Assert.Null(evt.CorrelationId);
    }

    [Fact]
    public void CorrelationId_CanBeSet()
    {
        var evt = BuildSnapshot() with { CorrelationId = "trace-abc-123" };
        Assert.Equal("trace-abc-123", evt.CorrelationId);
    }

    // ── Immutability (record) ──────────────────────────────────────────────

    [Fact]
    public void WithExpression_ProducesNewInstance()
    {
        var original = BuildSnapshot();
        var modified = original with { CurrentState = "completed" };
        Assert.NotSame(original, modified);
        Assert.Equal("queued", original.CurrentState);
        Assert.Equal("completed", modified.CurrentState);
    }

    // ── Helpers ───────────────────────────────────────────────────────────

    private static StoreOrderSnapshotted BuildSnapshot() =>
        new(
            EventId: Guid.NewGuid(),
            OrderId: Guid.NewGuid(),
            StoreId: Guid.NewGuid(),
            CustomerId: Guid.NewGuid(),
            PreviousState: "received",
            CurrentState: "queued",
            IsRush: false,
            IsAtRisk: false,
            PriorityBand: "normal",
            AggregateVersion: 1,
            OccurredAt: DateTimeOffset.UtcNow);
}

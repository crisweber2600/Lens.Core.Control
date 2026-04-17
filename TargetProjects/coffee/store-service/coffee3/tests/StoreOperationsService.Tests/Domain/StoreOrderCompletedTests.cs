using System.Text.Json;
using StoreOperationsService.Domain.Events;

namespace StoreOperationsService.Tests.Domain;

public sealed class StoreOrderCompletedTests
{
    // ── Schema version ─────────────────────────────────────────────────────

    [Fact]
    public void SchemaVersion_DefaultsToOne()
    {
        var evt = BuildCompleted();
        Assert.Equal(1, evt.SchemaVersion);
    }

    [Fact]
    public void SchemaVersion_IsNonZero()
    {
        var evt = BuildCompleted();
        Assert.True(evt.SchemaVersion > 0, "SchemaVersion must be positive");
    }

    // ── Required fields per decisions.md Q2 ───────────────────────────────

    [Fact]
    public void StoreOrderId_IsNotEmpty()
    {
        // store_order_id maps to OrderId
        var evt = BuildCompleted();
        Assert.NotEqual(Guid.Empty, evt.OrderId);
    }

    [Fact]
    public void CustomerOrderId_IsNotEmpty()
    {
        var evt = BuildCompleted();
        Assert.NotEqual(Guid.Empty, evt.CustomerOrderId);
    }

    [Fact]
    public void StoreId_IsNotEmpty()
    {
        var evt = BuildCompleted();
        Assert.NotEqual(Guid.Empty, evt.StoreId);
    }

    [Fact]
    public void CompletedAt_IsNotDefault()
    {
        var evt = BuildCompleted();
        Assert.NotEqual(default, evt.CompletedAt);
    }

    [Fact]
    public void CompletedBy_IsNotNullOrEmpty()
    {
        var evt = BuildCompleted();
        Assert.False(string.IsNullOrEmpty(evt.CompletedBy));
    }

    [Fact]
    public void CompletionMode_IsNotNullOrEmpty()
    {
        var evt = BuildCompleted();
        Assert.False(string.IsNullOrEmpty(evt.CompletionMode));
    }

    [Fact]
    public void AggregateVersion_IsPositive()
    {
        var evt = BuildCompleted();
        Assert.True(evt.AggregateVersion > 0);
    }

    // ── IDomainEvent contract ──────────────────────────────────────────────

    [Fact]
    public void ImplementsIDomainEvent()
    {
        IDomainEvent evt = BuildCompleted();
        Assert.NotEqual(Guid.Empty, evt.OrderId);
        Assert.Equal(1, evt.SchemaVersion);
    }

    // ── JSON serializability ───────────────────────────────────────────────

    [Fact]
    public void IsJsonSerializable_RoundTrip()
    {
        var evt = BuildCompleted();
        var json = JsonSerializer.Serialize(evt);
        var deserialized = JsonSerializer.Deserialize<StoreOrderCompleted>(json);
        Assert.NotNull(deserialized);
        Assert.Equal(evt.EventId, deserialized!.EventId);
        Assert.Equal(evt.OrderId, deserialized.OrderId);
        Assert.Equal(evt.CustomerOrderId, deserialized.CustomerOrderId);
        Assert.Equal(evt.CompletionMode, deserialized.CompletionMode);
        Assert.Equal(evt.SchemaVersion, deserialized.SchemaVersion);
    }

    [Fact]
    public void Json_ContainsSchemaVersionField()
    {
        var evt = BuildCompleted();
        var json = JsonSerializer.Serialize(evt);
        Assert.Contains("SchemaVersion", json);
    }

    [Fact]
    public void Json_ContainsCustomerOrderIdField()
    {
        var evt = BuildCompleted();
        var json = JsonSerializer.Serialize(evt);
        Assert.Contains("CustomerOrderId", json);
    }

    [Fact]
    public void Json_ContainsCompletionModeField()
    {
        var evt = BuildCompleted();
        var json = JsonSerializer.Serialize(evt);
        Assert.Contains("CompletionMode", json);
    }

    // ── Optional correlation id ────────────────────────────────────────────

    [Fact]
    public void CorrelationId_DefaultsToNull()
    {
        var evt = BuildCompleted();
        Assert.Null(evt.CorrelationId);
    }

    [Fact]
    public void CorrelationId_CanBeSet()
    {
        var evt = BuildCompleted() with { CorrelationId = "trace-xyz-456" };
        Assert.Equal("trace-xyz-456", evt.CorrelationId);
    }

    // ── Immutability (record) ──────────────────────────────────────────────

    [Fact]
    public void WithExpression_ProducesNewInstance()
    {
        var original = BuildCompleted();
        var modified = original with { CompletionMode = "pickup-confirmed" };
        Assert.NotSame(original, modified);
        Assert.Equal("handoff-confirmed", original.CompletionMode);
        Assert.Equal("pickup-confirmed", modified.CompletionMode);
    }

    // ── Helpers ───────────────────────────────────────────────────────────

    private static StoreOrderCompleted BuildCompleted() =>
        new(
            EventId: Guid.NewGuid(),
            OrderId: Guid.NewGuid(),
            CustomerOrderId: Guid.NewGuid(),
            StoreId: Guid.NewGuid(),
            CompletedAt: DateTimeOffset.UtcNow,
            CompletedBy: "operator-001",
            CompletionMode: "handoff-confirmed",
            AggregateVersion: 3,
            OccurredAt: DateTimeOffset.UtcNow);
}

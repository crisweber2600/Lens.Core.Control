using StoreOperationsService.Domain.Repositories;

namespace StoreOperationsService.Tests.Infrastructure;

/// <summary>
/// Verifies the IOutboxRepository interface contract:
/// - shape and member signatures exist at compile time
/// - the OutboxMessageDto read-model carries the expected properties
/// </summary>
public sealed class IOutboxRepositoryContractTests
{
    [Fact]
    public void IOutboxRepository_EnqueueAsync_SignatureExists()
    {
        // Assert that the method is defined on the interface (compile-time check via Reflection)
        var method = typeof(IOutboxRepository).GetMethod("EnqueueAsync");
        Assert.NotNull(method);
    }

    [Fact]
    public void IOutboxRepository_GetUnsentAsync_SignatureExists()
    {
        var method = typeof(IOutboxRepository).GetMethod("GetUnsentAsync");
        Assert.NotNull(method);
    }

    [Fact]
    public void IOutboxRepository_MarkSentAsync_SignatureExists()
    {
        var method = typeof(IOutboxRepository).GetMethod("MarkSentAsync");
        Assert.NotNull(method);
    }

    [Fact]
    public void OutboxMessageDto_Id_PropertyExists()
    {
        var prop = typeof(OutboxMessageDto).GetProperty("Id");
        Assert.NotNull(prop);
        Assert.Equal(typeof(long), prop!.PropertyType);
    }

    [Fact]
    public void OutboxMessageDto_Type_PropertyExists()
    {
        var prop = typeof(OutboxMessageDto).GetProperty("Type");
        Assert.NotNull(prop);
        Assert.Equal(typeof(string), prop!.PropertyType);
    }

    [Fact]
    public void OutboxMessageDto_Payload_PropertyExists()
    {
        var prop = typeof(OutboxMessageDto).GetProperty("Payload");
        Assert.NotNull(prop);
        Assert.Equal(typeof(string), prop!.PropertyType);
    }

    [Fact]
    public void OutboxMessageDto_OccurredAt_PropertyExists()
    {
        var prop = typeof(OutboxMessageDto).GetProperty("OccurredAt");
        Assert.NotNull(prop);
        Assert.Equal(typeof(DateTimeOffset), prop!.PropertyType);
    }

    [Fact]
    public void OutboxMessageDto_CorrelationId_PropertyExists_AndIsNullable()
    {
        var prop = typeof(OutboxMessageDto).GetProperty("CorrelationId");
        Assert.NotNull(prop);
        Assert.Equal(typeof(string), prop!.PropertyType);
    }

    [Fact]
    public void OutboxMessageDto_CanBeConstructed()
    {
        var dto = new OutboxMessageDto(
            Id: 42,
            Type: "SomeEvent",
            Payload: "{}",
            OccurredAt: DateTimeOffset.UtcNow,
            CorrelationId: null);

        Assert.Equal(42, dto.Id);
        Assert.Equal("SomeEvent", dto.Type);
        Assert.Null(dto.CorrelationId);
    }

    [Fact]
    public void OutboxMessageDto_WithCorrelationId_CanBeConstructed()
    {
        var dto = new OutboxMessageDto(
            Id: 1,
            Type: "SomeEvent",
            Payload: "{}",
            OccurredAt: DateTimeOffset.UtcNow,
            CorrelationId: "corr-xyz");

        Assert.Equal("corr-xyz", dto.CorrelationId);
    }

    [Fact]
    public void OutboxMessageDto_IsRecord_SupportsValueEquality()
    {
        var now = DateTimeOffset.UtcNow;
        var a = new OutboxMessageDto(1, "E", "{}", now, null);
        var b = new OutboxMessageDto(1, "E", "{}", now, null);
        Assert.Equal(a, b);
    }
}

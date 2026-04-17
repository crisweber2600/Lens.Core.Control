using Microsoft.EntityFrameworkCore;
using StoreOperationsService.Infrastructure;
using StoreOperationsService.Infrastructure.Entities;

namespace StoreOperationsService.Tests.Infrastructure;

public sealed class OutboxMessageTests
{
    private static StoreOperationsDbContext CreateInMemoryContext(string dbName)
    {
        var options = new DbContextOptionsBuilder<StoreOperationsDbContext>()
            .UseInMemoryDatabase(dbName)
            .Options;
        return new StoreOperationsDbContext(options);
    }

    // --- Entity property defaults ---

    [Fact]
    public void OutboxMessage_Type_DefaultsToEmptyString()
    {
        var msg = new OutboxMessage();
        Assert.Equal(string.Empty, msg.Type);
    }

    [Fact]
    public void OutboxMessage_Payload_DefaultsToEmptyString()
    {
        var msg = new OutboxMessage();
        Assert.Equal(string.Empty, msg.Payload);
    }

    [Fact]
    public void OutboxMessage_SentAt_DefaultsToNull()
    {
        var msg = new OutboxMessage();
        Assert.Null(msg.SentAt);
    }

    [Fact]
    public void OutboxMessage_CorrelationId_DefaultsToNull()
    {
        var msg = new OutboxMessage();
        Assert.Null(msg.CorrelationId);
    }

    // --- EF persistence ---

    [Fact]
    public async Task OutboxMessages_DbSet_IsAvailable()
    {
        await using var ctx = CreateInMemoryContext(nameof(OutboxMessages_DbSet_IsAvailable));
        Assert.NotNull(ctx.OutboxMessages);
    }

    [Fact]
    public async Task OutboxMessage_CanAddAndRetrieve()
    {
        await using var ctx = CreateInMemoryContext(nameof(OutboxMessage_CanAddAndRetrieve));
        var now = DateTimeOffset.UtcNow;

        ctx.OutboxMessages.Add(new OutboxMessage
        {
            Type = "StoreOperationsService.Domain.Events.StoreOrderCompleted",
            Payload = "{\"eventId\":\"00000000-0000-0000-0000-000000000001\"}",
            OccurredAt = now
        });
        await ctx.SaveChangesAsync();

        var messages = await ctx.OutboxMessages.ToListAsync();
        Assert.Single(messages);
        Assert.Equal("StoreOperationsService.Domain.Events.StoreOrderCompleted", messages[0].Type);
    }

    [Fact]
    public async Task OutboxMessage_SentAt_IsNullAfterInsert()
    {
        await using var ctx = CreateInMemoryContext(nameof(OutboxMessage_SentAt_IsNullAfterInsert));

        ctx.OutboxMessages.Add(new OutboxMessage
        {
            Type = "SomeEvent",
            Payload = "{}",
            OccurredAt = DateTimeOffset.UtcNow
        });
        await ctx.SaveChangesAsync();

        var msg = await ctx.OutboxMessages.FirstAsync();
        Assert.Null(msg.SentAt);
    }

    [Fact]
    public async Task OutboxMessage_SentAt_CanBeMarkedSent()
    {
        await using var ctx = CreateInMemoryContext(nameof(OutboxMessage_SentAt_CanBeMarkedSent));
        var now = DateTimeOffset.UtcNow;

        ctx.OutboxMessages.Add(new OutboxMessage
        {
            Type = "SomeEvent",
            Payload = "{}",
            OccurredAt = now
        });
        await ctx.SaveChangesAsync();

        var msg = await ctx.OutboxMessages.FirstAsync();
        msg.SentAt = now.AddSeconds(2);
        await ctx.SaveChangesAsync();

        var updated = await ctx.OutboxMessages.FindAsync(msg.Id);
        Assert.NotNull(updated!.SentAt);
    }

    [Fact]
    public async Task OutboxMessage_CorrelationId_CanBeStored()
    {
        await using var ctx = CreateInMemoryContext(nameof(OutboxMessage_CorrelationId_CanBeStored));

        ctx.OutboxMessages.Add(new OutboxMessage
        {
            Type = "SomeEvent",
            Payload = "{}",
            OccurredAt = DateTimeOffset.UtcNow,
            CorrelationId = "corr-001"
        });
        await ctx.SaveChangesAsync();

        var msg = await ctx.OutboxMessages.FirstAsync();
        Assert.Equal("corr-001", msg.CorrelationId);
    }

    [Fact]
    public async Task OutboxMessages_CanQueryUnsentByNullSentAt()
    {
        await using var ctx = CreateInMemoryContext(nameof(OutboxMessages_CanQueryUnsentByNullSentAt));
        var now = DateTimeOffset.UtcNow;

        ctx.OutboxMessages.AddRange(
            new OutboxMessage { Type = "E1", Payload = "{}", OccurredAt = now },
            new OutboxMessage { Type = "E2", Payload = "{}", OccurredAt = now, SentAt = now },
            new OutboxMessage { Type = "E3", Payload = "{}", OccurredAt = now }
        );
        await ctx.SaveChangesAsync();

        var unsent = await ctx.OutboxMessages
            .Where(m => m.SentAt == null)
            .ToListAsync();

        Assert.Equal(2, unsent.Count);
    }

    [Fact]
    public async Task OutboxMessages_CanQueryOrderedByOccurredAt()
    {
        await using var ctx = CreateInMemoryContext(nameof(OutboxMessages_CanQueryOrderedByOccurredAt));
        var base_ = DateTimeOffset.UtcNow;

        ctx.OutboxMessages.AddRange(
            new OutboxMessage { Type = "E1", Payload = "{}", OccurredAt = base_.AddSeconds(2) },
            new OutboxMessage { Type = "E2", Payload = "{}", OccurredAt = base_.AddSeconds(1) },
            new OutboxMessage { Type = "E3", Payload = "{}", OccurredAt = base_ }
        );
        await ctx.SaveChangesAsync();

        var ordered = await ctx.OutboxMessages
            .OrderBy(m => m.OccurredAt)
            .Select(m => m.Type)
            .ToListAsync();

        Assert.Equal(["E3", "E2", "E1"], ordered);
    }

    [Fact]
    public async Task OutboxMessage_MultipleMessages_HaveUniqueIds()
    {
        await using var ctx = CreateInMemoryContext(nameof(OutboxMessage_MultipleMessages_HaveUniqueIds));
        var now = DateTimeOffset.UtcNow;

        ctx.OutboxMessages.AddRange(
            new OutboxMessage { Type = "E1", Payload = "{}", OccurredAt = now },
            new OutboxMessage { Type = "E2", Payload = "{}", OccurredAt = now }
        );
        await ctx.SaveChangesAsync();

        var ids = await ctx.OutboxMessages.Select(m => m.Id).ToListAsync();
        Assert.Equal(2, ids.Distinct().Count());
    }
}

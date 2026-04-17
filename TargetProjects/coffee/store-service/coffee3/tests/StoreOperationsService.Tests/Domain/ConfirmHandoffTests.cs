using StoreOperationsService.Domain;
using StoreOperationsService.Domain.Exceptions;

namespace StoreOperationsService.Tests.Domain;

public class ConfirmHandoffTests
{
    private static StoreOrder ReadyOrder()
    {
        var order = new StoreOrder(Guid.NewGuid(), Guid.NewGuid(), DateTimeOffset.UtcNow);
        order.Transition(OrderLifecycleState.Queued);
        order.Transition(OrderLifecycleState.InProgress);
        order.Transition(OrderLifecycleState.Ready);
        return order;
    }

    // ── Happy path ─────────────────────────────────────────────────────────

    [Fact]
    public void ConfirmHandoff_SetsStateToCompleted()
    {
        var order = ReadyOrder();
        order.ConfirmHandoff();
        Assert.Equal(OrderLifecycleState.Completed, order.LifecycleState);
    }

    [Fact]
    public void ConfirmHandoff_SetsCompletedAt()
    {
        var before = DateTimeOffset.UtcNow;
        var order = ReadyOrder();
        order.ConfirmHandoff();
        Assert.NotNull(order.CompletedAt);
        Assert.True(order.CompletedAt >= before);
    }

    [Fact]
    public void ConfirmHandoff_CompletedAt_MatchesUpdatedAt()
    {
        var order = ReadyOrder();
        order.ConfirmHandoff();
        Assert.Equal(order.CompletedAt, order.UpdatedAt);
    }

    [Fact]
    public void ConfirmHandoff_CompletedAt_IsNull_BeforeHandoff()
    {
        var order = ReadyOrder();
        Assert.Null(order.CompletedAt);
    }

    // ── Invalid state transitions ──────────────────────────────────────────

    [Fact]
    public void ConfirmHandoff_WhenNotReady_Received_Throws()
    {
        var order = new StoreOrder(Guid.NewGuid(), Guid.NewGuid(), DateTimeOffset.UtcNow);
        Assert.Throws<InvalidTransitionException>(() => order.ConfirmHandoff());
    }

    [Fact]
    public void ConfirmHandoff_WhenNotReady_Queued_Throws()
    {
        var order = new StoreOrder(Guid.NewGuid(), Guid.NewGuid(), DateTimeOffset.UtcNow);
        order.Transition(OrderLifecycleState.Queued);
        Assert.Throws<InvalidTransitionException>(() => order.ConfirmHandoff());
    }

    [Fact]
    public void ConfirmHandoff_WhenNotReady_InProgress_Throws()
    {
        var order = new StoreOrder(Guid.NewGuid(), Guid.NewGuid(), DateTimeOffset.UtcNow);
        order.Transition(OrderLifecycleState.Queued);
        order.Transition(OrderLifecycleState.InProgress);
        Assert.Throws<InvalidTransitionException>(() => order.ConfirmHandoff());
    }

    [Fact]
    public void ConfirmHandoff_WhenAlreadyCompleted_Throws()
    {
        var order = ReadyOrder();
        order.ConfirmHandoff();
        Assert.Throws<InvalidTransitionException>(() => order.ConfirmHandoff());
    }

    [Fact]
    public void ConfirmHandoff_WhenCancelled_Throws()
    {
        var order = new StoreOrder(Guid.NewGuid(), Guid.NewGuid(), DateTimeOffset.UtcNow);
        order.Transition(OrderLifecycleState.Cancelled);
        Assert.Throws<InvalidTransitionException>(() => order.ConfirmHandoff());
    }
}

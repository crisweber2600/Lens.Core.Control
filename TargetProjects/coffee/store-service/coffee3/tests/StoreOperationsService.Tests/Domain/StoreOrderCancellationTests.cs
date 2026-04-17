using StoreOperationsService.Domain;
using StoreOperationsService.Domain.Exceptions;

namespace StoreOperationsService.Tests.Domain;

/// <summary>
/// Unit tests for <see cref="StoreOrder.Cancel"/>.
/// Covers state transitions, audit-property population, and cancellation-stage classification.
/// </summary>
public sealed class StoreOrderCancellationTests
{
    // ── Helpers ────────────────────────────────────────────────────────────

    private static StoreOrder ReceivedOrder() =>
        new(Guid.NewGuid(), Guid.NewGuid(), DateTimeOffset.UtcNow);

    private static StoreOrder OrderInState(OrderLifecycleState target)
    {
        var o = ReceivedOrder();
        if (target == OrderLifecycleState.Received) return o;
        if (target == OrderLifecycleState.Queued)   { o.Transition(OrderLifecycleState.Queued);      return o; }
        if (target == OrderLifecycleState.InProgress){ o.Transition(OrderLifecycleState.Queued); o.Transition(OrderLifecycleState.InProgress); return o; }
        if (target == OrderLifecycleState.Ready)    { o.Transition(OrderLifecycleState.Queued); o.Transition(OrderLifecycleState.InProgress); o.Transition(OrderLifecycleState.Ready); return o; }
        throw new InvalidOperationException($"Unsupported setup state: {target}");
    }

    // ── State-transition tests ─────────────────────────────────────────────

    [Fact]
    public void Cancel_FromReceived_SetsStateToCancelled()
    {
        var order = OrderInState(OrderLifecycleState.Received);
        order.Cancel(CancellationReasonCode.CustomerRequest, "operator-1");
        Assert.Equal(OrderLifecycleState.Cancelled, order.LifecycleState);
    }

    [Fact]
    public void Cancel_FromQueued_SetsStateToCancelled()
    {
        var order = OrderInState(OrderLifecycleState.Queued);
        order.Cancel(CancellationReasonCode.ItemUnavailable, "system");
        Assert.Equal(OrderLifecycleState.Cancelled, order.LifecycleState);
    }

    [Fact]
    public void Cancel_FromInProgress_SetsStateToCancelled()
    {
        var order = OrderInState(OrderLifecycleState.InProgress);
        order.Cancel(CancellationReasonCode.OperationalError, "supervisor");
        Assert.Equal(OrderLifecycleState.Cancelled, order.LifecycleState);
    }

    [Fact]
    public void Cancel_FromReady_SetsStateToCancelled()
    {
        var order = OrderInState(OrderLifecycleState.Ready);
        order.Cancel(CancellationReasonCode.OrderTimeout, "system");
        Assert.Equal(OrderLifecycleState.Cancelled, order.LifecycleState);
    }

    // ── Terminal-state guards ──────────────────────────────────────────────

    [Fact]
    public void Cancel_FromCompleted_ThrowsInvalidTransitionException()
    {
        var order = OrderInState(OrderLifecycleState.Ready);
        order.ConfirmHandoff();
        Assert.Throws<InvalidTransitionException>(() =>
            order.Cancel(CancellationReasonCode.CustomerRequest, "late-actor"));
    }

    [Fact]
    public void Cancel_WhenAlreadyCancelled_ThrowsInvalidTransitionException()
    {
        var order = ReceivedOrder();
        order.Cancel(CancellationReasonCode.CustomerRequest, "first");
        Assert.Throws<InvalidTransitionException>(() =>
            order.Cancel(CancellationReasonCode.CustomerRequest, "second"));
    }

    // ── Stage classification ───────────────────────────────────────────────

    [Fact]
    public void Cancel_FromReceived_StageIsPreStart()
    {
        var order = OrderInState(OrderLifecycleState.Received);
        order.Cancel(CancellationReasonCode.CustomerRequest, "actor");
        Assert.Equal("pre-start", order.CancelledStage);
    }

    [Fact]
    public void Cancel_FromQueued_StageIsPreStart()
    {
        var order = OrderInState(OrderLifecycleState.Queued);
        order.Cancel(CancellationReasonCode.CustomerRequest, "actor");
        Assert.Equal("pre-start", order.CancelledStage);
    }

    [Fact]
    public void Cancel_FromInProgress_StageIsInProgress()
    {
        var order = OrderInState(OrderLifecycleState.InProgress);
        order.Cancel(CancellationReasonCode.OperationalError, "actor");
        Assert.Equal("in-progress", order.CancelledStage);
    }

    [Fact]
    public void Cancel_FromReady_StageIsPostReady()
    {
        var order = OrderInState(OrderLifecycleState.Ready);
        order.Cancel(CancellationReasonCode.OrderTimeout, "actor");
        Assert.Equal("post-ready", order.CancelledStage);
    }

    // ── Audit-property population ──────────────────────────────────────────

    [Fact]
    public void Cancel_SetsCancelledAt()
    {
        var order = ReceivedOrder();
        order.Cancel(CancellationReasonCode.CustomerRequest, "actor");
        Assert.NotNull(order.CancelledAt);
    }

    [Fact]
    public void Cancel_CancelledAtMatchesUpdatedAt()
    {
        var order = ReceivedOrder();
        order.Cancel(CancellationReasonCode.CustomerRequest, "actor");
        Assert.Equal(order.UpdatedAt, order.CancelledAt);
    }

    [Fact]
    public void Cancel_SetsCancellationReasonToReasonCodeName()
    {
        var order = ReceivedOrder();
        order.Cancel(CancellationReasonCode.ItemUnavailable, "actor");
        Assert.Equal("ItemUnavailable", order.CancellationReason);
    }

    [Fact]
    public void Cancel_SetsCancelledBy()
    {
        var order = ReceivedOrder();
        order.Cancel(CancellationReasonCode.CustomerRequest, "operator-42");
        Assert.Equal("operator-42", order.CancelledBy);
    }

    [Fact]
    public void Cancel_DoesNotSetCompletedAt()
    {
        var order = ReceivedOrder();
        order.Cancel(CancellationReasonCode.CustomerRequest, "actor");
        Assert.Null(order.CompletedAt);
    }
}

using StoreOperationsService.Domain;
using StoreOperationsService.Domain.Exceptions;

namespace StoreOperationsService.Tests.Domain;

public class StoreOrderTransitionTests
{
    // ── Helpers ────────────────────────────────────────────────────────────

    private static StoreOrder NewOrder() =>
        new(Guid.NewGuid(), Guid.NewGuid(), DateTimeOffset.UtcNow);

    // ── Valid forward-path transitions ─────────────────────────────────────

    [Fact]
    public void Transition_Received_To_Queued_DoesNotThrow()
    {
        var order = NewOrder();
        // no exception
        order.Transition(OrderLifecycleState.Queued);
        Assert.Equal(OrderLifecycleState.Queued, order.LifecycleState);
    }

    [Fact]
    public void Transition_Queued_To_InProgress_DoesNotThrow()
    {
        var order = NewOrder();
        order.Transition(OrderLifecycleState.Queued);
        order.Transition(OrderLifecycleState.InProgress);
        Assert.Equal(OrderLifecycleState.InProgress, order.LifecycleState);
    }

    [Fact]
    public void Transition_InProgress_To_Ready_SetsStateReady()
    {
        var order = NewOrder();
        order.Transition(OrderLifecycleState.Queued);
        order.Transition(OrderLifecycleState.InProgress);
        order.Transition(OrderLifecycleState.Ready);
        Assert.Equal(OrderLifecycleState.Ready, order.LifecycleState);
    }

    [Fact]
    public void Transition_Ready_To_Completed_DoesNotThrow()
    {
        var order = NewOrder();
        order.Transition(OrderLifecycleState.Queued);
        order.Transition(OrderLifecycleState.InProgress);
        order.Transition(OrderLifecycleState.Ready);
        order.Transition(OrderLifecycleState.Completed);
        Assert.Equal(OrderLifecycleState.Completed, order.LifecycleState);
    }

    // ── Valid cancellation paths ────────────────────────────────────────────

    [Theory]
    [InlineData(new OrderLifecycleState[] { })]                                                         // Received → Cancelled
    [InlineData(new[] { OrderLifecycleState.Queued })]                                                 // Queued → Cancelled
    [InlineData(new[] { OrderLifecycleState.Queued, OrderLifecycleState.InProgress })]                 // InProgress → Cancelled
    [InlineData(new[] { OrderLifecycleState.Queued, OrderLifecycleState.InProgress, OrderLifecycleState.Ready })] // Ready → Cancelled
    public void Transition_ToCancelled_FromAnyNonTerminalState_DoesNotThrow(
        OrderLifecycleState[] priorTransitions)
    {
        var order = NewOrder();
        foreach (var s in priorTransitions)
            order.Transition(s);

        order.Transition(OrderLifecycleState.Cancelled);
        Assert.Equal(OrderLifecycleState.Cancelled, order.LifecycleState);
    }

    // ── AD-4: Completed → Cancelled is explicitly invalid ─────────────────

    [Fact]
    public void Transition_Completed_To_Cancelled_ThrowsInvalidTransitionException()
    {
        var order = NewOrder();
        order.Transition(OrderLifecycleState.Queued);
        order.Transition(OrderLifecycleState.InProgress);
        order.Transition(OrderLifecycleState.Ready);
        order.Transition(OrderLifecycleState.Completed);

        var ex = Assert.Throws<InvalidTransitionException>(
            () => order.Transition(OrderLifecycleState.Cancelled));

        Assert.Equal(OrderLifecycleState.Completed, ex.FromState);
        Assert.Equal(OrderLifecycleState.Cancelled, ex.ToState);
        Assert.Contains("Completed → Cancelled", ex.Message);
    }

    // ── Terminal state: Cancelled → any → throws ──────────────────────────

    [Theory]
    [InlineData(OrderLifecycleState.Received)]
    [InlineData(OrderLifecycleState.Queued)]
    [InlineData(OrderLifecycleState.InProgress)]
    [InlineData(OrderLifecycleState.Ready)]
    [InlineData(OrderLifecycleState.Completed)]
    [InlineData(OrderLifecycleState.Cancelled)]
    public void Transition_FromCancelled_ToAnyState_ThrowsInvalidTransitionException(
        OrderLifecycleState targetState)
    {
        var order = NewOrder();
        order.Transition(OrderLifecycleState.Cancelled);

        var ex = Assert.Throws<InvalidTransitionException>(
            () => order.Transition(targetState));

        Assert.Equal(order.OrderId, ex.OrderId);
        Assert.Equal(OrderLifecycleState.Cancelled, ex.FromState);
        Assert.Equal(targetState, ex.ToState);
    }

    // ── Terminal state: Completed → any → throws ──────────────────────────

    [Theory]
    [InlineData(OrderLifecycleState.Received)]
    [InlineData(OrderLifecycleState.Queued)]
    [InlineData(OrderLifecycleState.InProgress)]
    [InlineData(OrderLifecycleState.Ready)]
    [InlineData(OrderLifecycleState.Completed)]
    [InlineData(OrderLifecycleState.Cancelled)]
    public void Transition_FromCompleted_ToAnyState_ThrowsInvalidTransitionException(
        OrderLifecycleState targetState)
    {
        var order = NewOrder();
        order.Transition(OrderLifecycleState.Queued);
        order.Transition(OrderLifecycleState.InProgress);
        order.Transition(OrderLifecycleState.Ready);
        order.Transition(OrderLifecycleState.Completed);

        var ex = Assert.Throws<InvalidTransitionException>(
            () => order.Transition(targetState));

        Assert.Equal(order.OrderId, ex.OrderId);
        Assert.Equal(OrderLifecycleState.Completed, ex.FromState);
        Assert.Equal(targetState, ex.ToState);
    }

    // ── Invalid forward skips ──────────────────────────────────────────────

    [Theory]
    [InlineData(OrderLifecycleState.Received, OrderLifecycleState.InProgress)]
    [InlineData(OrderLifecycleState.Received, OrderLifecycleState.Ready)]
    [InlineData(OrderLifecycleState.Received, OrderLifecycleState.Completed)]
    [InlineData(OrderLifecycleState.Queued,   OrderLifecycleState.Ready)]
    [InlineData(OrderLifecycleState.Queued,   OrderLifecycleState.Completed)]
    public void Transition_SkippingStates_ThrowsInvalidTransitionException(
        OrderLifecycleState fromState,
        OrderLifecycleState toState)
    {
        var order = NewOrder();

        // Advance to fromState (fromState is always Received or Queued here)
        if (fromState == OrderLifecycleState.Queued)
            order.Transition(OrderLifecycleState.Queued);

        Assert.Throws<InvalidTransitionException>(() => order.Transition(toState));
    }

    // ── Exception carries correct orderId ─────────────────────────────────

    [Fact]
    public void InvalidTransitionException_ContainsOrderId()
    {
        var order = NewOrder();
        order.Transition(OrderLifecycleState.Cancelled);

        var ex = Assert.Throws<InvalidTransitionException>(
            () => order.Transition(OrderLifecycleState.Queued));

        Assert.Equal(order.OrderId, ex.OrderId);
        Assert.Contains(order.OrderId.ToString(), ex.Message);
    }

    // ── Aggregate initial state ────────────────────────────────────────────

    [Fact]
    public void NewOrder_InitialState_IsReceived()
    {
        var order = NewOrder();
        Assert.Equal(OrderLifecycleState.Received, order.LifecycleState);
    }

    [Fact]
    public void NewOrder_OperationalModifiers_AreDefault()
    {
        var order = NewOrder();
        Assert.False(order.OperationalModifiers.IsRush);
        Assert.False(order.OperationalModifiers.IsAtRisk);
        Assert.Equal(PriorityBand.Standard, order.OperationalModifiers.PriorityBand);
    }
}

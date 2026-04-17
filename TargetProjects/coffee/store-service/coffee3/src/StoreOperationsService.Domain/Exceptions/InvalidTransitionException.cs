namespace StoreOperationsService.Domain.Exceptions;

public sealed class InvalidTransitionException : Exception
{
    public Guid OrderId { get; }
    public OrderLifecycleState FromState { get; }
    public OrderLifecycleState ToState { get; }

    public InvalidTransitionException(
        Guid orderId,
        OrderLifecycleState fromState,
        OrderLifecycleState toState)
        : base($"Order {orderId}: invalid transition {fromState} → {toState}.")
    {
        OrderId = orderId;
        FromState = fromState;
        ToState = toState;
    }
}

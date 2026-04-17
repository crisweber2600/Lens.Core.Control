namespace StoreOperationsService.Domain;

public enum OrderLifecycleState
{
    Received,
    Queued,
    InProgress,
    Ready,
    Completed,
    Cancelled
}

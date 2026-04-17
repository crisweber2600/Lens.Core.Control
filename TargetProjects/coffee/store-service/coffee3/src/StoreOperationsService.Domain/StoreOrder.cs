using StoreOperationsService.Domain.Exceptions;

namespace StoreOperationsService.Domain;

/// <summary>
/// StoreOrder aggregate — lifecycle state and operational modifiers.
/// Pure domain model (no persistence, no repositories, no DbContext).
/// </summary>
public sealed class StoreOrder
{
    // ── Identity ───────────────────────────────────────────────────────────

    public Guid OrderId { get; }
    public Guid CustomerId { get; }

    // ── State ──────────────────────────────────────────────────────────────

    public OrderLifecycleState LifecycleState { get; private set; }
    public OperationalModifiers OperationalModifiers { get; private set; }

    // ── Audit ──────────────────────────────────────────────────────────────

    public DateTimeOffset CreatedAt { get; }
    public DateTimeOffset UpdatedAt { get; private set; }

    // ── Allowed transitions (AD-1, AD-4) ──────────────────────────────────
    //
    //  Received  → Queued | Cancelled
    //  Queued    → InProgress | Cancelled
    //  InProgress→ Ready | Cancelled
    //  Ready     → Completed | Cancelled
    //  Completed → (terminal — no transitions allowed)
    //  Cancelled → (terminal — no transitions allowed)

    private static readonly IReadOnlyDictionary<OrderLifecycleState, IReadOnlySet<OrderLifecycleState>> AllowedTransitions =
        new Dictionary<OrderLifecycleState, IReadOnlySet<OrderLifecycleState>>
        {
            [OrderLifecycleState.Received]   = new HashSet<OrderLifecycleState> { OrderLifecycleState.Queued,     OrderLifecycleState.Cancelled },
            [OrderLifecycleState.Queued]     = new HashSet<OrderLifecycleState> { OrderLifecycleState.InProgress, OrderLifecycleState.Cancelled },
            [OrderLifecycleState.InProgress] = new HashSet<OrderLifecycleState> { OrderLifecycleState.Ready,      OrderLifecycleState.Cancelled },
            [OrderLifecycleState.Ready]      = new HashSet<OrderLifecycleState> { OrderLifecycleState.Completed,  OrderLifecycleState.Cancelled },
            [OrderLifecycleState.Completed]  = new HashSet<OrderLifecycleState>(),
            [OrderLifecycleState.Cancelled]  = new HashSet<OrderLifecycleState>(),
        };

    // ── Constructor ────────────────────────────────────────────────────────

    public StoreOrder(Guid orderId, Guid customerId, DateTimeOffset createdAt)
    {
        OrderId = orderId;
        CustomerId = customerId;
        LifecycleState = OrderLifecycleState.Received;
        OperationalModifiers = OperationalModifiers.Default;
        CreatedAt = createdAt;
        UpdatedAt = createdAt;
    }

    // ── Lifecycle transition ───────────────────────────────────────────────

    /// <summary>
    /// Advances the order to <paramref name="newState"/>.
    /// Throws <see cref="InvalidTransitionException"/> when the transition is
    /// not permitted by the lifecycle table.
    /// </summary>
    public void Transition(OrderLifecycleState newState)
    {
        if (!AllowedTransitions.TryGetValue(LifecycleState, out var allowed) ||
            !allowed.Contains(newState))
        {
            throw new InvalidTransitionException(OrderId, LifecycleState, newState);
        }

        LifecycleState = newState;
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    // ── Operational modifiers (mutable per queue evaluation) ──────────────

    public void UpdateModifiers(OperationalModifiers modifiers)
    {
        OperationalModifiers = modifiers ?? throw new ArgumentNullException(nameof(modifiers));
        UpdatedAt = DateTimeOffset.UtcNow;
    }
}

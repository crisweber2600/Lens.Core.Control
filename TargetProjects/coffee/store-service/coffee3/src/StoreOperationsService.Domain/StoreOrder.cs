using StoreOperationsService.Domain.Exceptions;
using StoreOperationsService.Domain.ValueObjects;

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
    public DateTimeOffset? CompletedAt { get; private set; }

    // ── Cancellation audit ─────────────────────────────────────────────────

    public DateTimeOffset? CancelledAt { get; private set; }
    public string? CancellationReason { get; private set; }
    public string? CancelledBy { get; private set; }
    public string? CancelledStage { get; private set; }

    // ── Rush designation ───────────────────────────────────────────────────

    /// <summary>
    /// The priority band in effect before rush was applied.
    /// Restored when rush is removed.
    /// </summary>
    public PriorityBand OriginalBand { get; private set; } = PriorityBand.Standard;

    // ── Order type ─────────────────────────────────────────────────────────

    /// <summary>
    /// Classifies the kind of item in this order (Drink, Food, Addon, Unknown).
    /// Drives the at-risk threshold selection.
    /// </summary>
    public OrderType OrderType { get; private set; } = OrderType.Unknown;

    // ── At-risk timing ─────────────────────────────────────────────────────

    /// <summary>
    /// The instant the order entered the <see cref="OrderLifecycleState.Queued"/> state.
    /// <c>null</c> until the order is queued.
    /// </summary>
    public DateTimeOffset? QueuedAt { get; private set; }

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

    private StoreOrder(
        Guid orderId,
        Guid customerId,
        OrderLifecycleState lifecycleState,
        OperationalModifiers operationalModifiers,
        PriorityBand originalBand,
        DateTimeOffset createdAt,
        DateTimeOffset updatedAt,
        OrderType orderType,
        DateTimeOffset? queuedAt)
    {
        OrderId = orderId;
        CustomerId = customerId;
        LifecycleState = lifecycleState;
        OperationalModifiers = operationalModifiers;
        OriginalBand = originalBand;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
        OrderType = orderType;
        QueuedAt = queuedAt;
    }

    /// <summary>
    /// Reconstitutes a <see cref="StoreOrder"/> from persisted snapshot data.
    /// </summary>
    public static StoreOrder Reconstitute(
        Guid orderId,
        Guid customerId,
        OrderLifecycleState lifecycleState,
        OperationalModifiers operationalModifiers,
        DateTimeOffset createdAt,
        DateTimeOffset updatedAt,
        OrderType orderType = OrderType.Unknown,
        DateTimeOffset? queuedAt = null) =>
        new(orderId, customerId, lifecycleState, operationalModifiers,
            operationalModifiers.IsRush ? PriorityBand.Standard : operationalModifiers.PriorityBand,
            createdAt, updatedAt, orderType, queuedAt);

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

        if (newState == OrderLifecycleState.Queued)
            QueuedAt = UpdatedAt;

        if (newState == OrderLifecycleState.InProgress)
            ClearAtRisk();
    }

    // ── At-risk flag ───────────────────────────────────────────────────────

    /// <summary>
    /// Marks the order as at-risk.  No-op when already flagged.
    /// </summary>
    public void SetAtRisk()
    {
        if (OperationalModifiers.IsAtRisk) return;
        OperationalModifiers = OperationalModifiers.WithAtRisk(true);
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    /// <summary>
    /// Clears the at-risk flag.  No-op when not flagged.
    /// Called automatically when the order transitions to InProgress.
    /// </summary>
    public void ClearAtRisk()
    {
        if (!OperationalModifiers.IsAtRisk) return;
        OperationalModifiers = OperationalModifiers.WithAtRisk(false);
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    /// <summary>
    /// Sets the order type.
    /// </summary>
    public void SetOrderType(OrderType orderType)
    {
        OrderType = orderType;
    }

    // ── Operational modifiers (mutable per queue evaluation) ──────────────

    public void UpdateModifiers(OperationalModifiers modifiers)
    {
        OperationalModifiers = modifiers ?? throw new ArgumentNullException(nameof(modifiers));
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    // ── Handoff confirmation ───────────────────────────────────────────────

    /// <summary>
    /// Confirms handoff of a ready order, transitioning it to
    /// <see cref="OrderLifecycleState.Completed"/> and recording <see cref="CompletedAt"/>.
    /// </summary>
    /// <exception cref="Exceptions.InvalidTransitionException">
    /// Thrown when the order is not in the <c>Ready</c> state.
    /// </exception>
    public void ConfirmHandoff()
    {
        Transition(OrderLifecycleState.Completed);
        CompletedAt = UpdatedAt; // same instant set by Transition()
    }

    // ── Cancellation ───────────────────────────────────────────────────────

    /// <summary>
    /// Cancels the order from any non-terminal state, recording the reason, actor, and stage.
    /// Throws <see cref="Exceptions.InvalidTransitionException"/> when called on
    /// <c>Completed</c> or already <c>Cancelled</c> orders.
    /// </summary>
    /// <param name="reasonCode">Structured reason for the cancellation.</param>
    /// <param name="cancelledBy">Identity of the actor (user, service) performing the cancellation.</param>
    public void Cancel(CancellationReasonCode reasonCode, string cancelledBy)
    {
        // Compute the stage from current state BEFORE the transition guard fires.
        // Terminal states (Completed, Cancelled) fall through to the Transition() throw.
        var stage = LifecycleState switch
        {
            OrderLifecycleState.Received or OrderLifecycleState.Queued => "pre-start",
            OrderLifecycleState.InProgress                             => "in-progress",
            OrderLifecycleState.Ready                                  => "post-ready",
            _                                                          => null,
        };

        Transition(OrderLifecycleState.Cancelled); // throws for terminal states

        CancelledAt        = UpdatedAt;            // same instant set by Transition()
        CancellationReason = reasonCode.ToString();
        CancelledBy        = cancelledBy;
        CancelledStage     = stage!;
    }

    // ── Rush designation ───────────────────────────────────────────────────

    /// <summary>
    /// Marks the order as rush priority, elevating its band to
    /// <see cref="PriorityBand.Rush"/>. Idempotent when the order is already rush.
    /// </summary>
    /// <exception cref="InvalidOperationException">
    /// Thrown when the order is in a terminal state (Completed or Cancelled).
    /// </exception>
    public void DesignateRush(string designatedBy)
    {
        if (LifecycleState is OrderLifecycleState.Completed or OrderLifecycleState.Cancelled)
            throw new InvalidOperationException(
                $"Order {OrderId}: rush designation is not permitted in terminal state {LifecycleState}.");

        if (OperationalModifiers.IsRush)
            return; // idempotent

        OriginalBand = OperationalModifiers.PriorityBand;
        OperationalModifiers = OperationalModifiers with { IsRush = true, PriorityBand = PriorityBand.Rush };
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    /// <summary>
    /// Removes rush designation and restores the order to <see cref="OriginalBand"/>.
    /// Idempotent when the order is not currently rush.
    /// </summary>
    public void RemoveRushDesignation(string removedBy)
    {
        if (!OperationalModifiers.IsRush)
            return; // idempotent

        OperationalModifiers = OperationalModifiers with { IsRush = false, PriorityBand = OriginalBand };
        UpdatedAt = DateTimeOffset.UtcNow;
    }
}

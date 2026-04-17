using Microsoft.EntityFrameworkCore;
using StoreOperationsService.Domain.Repositories;
using StoreOperationsService.Infrastructure.Entities;

namespace StoreOperationsService.Infrastructure.Repositories;

/// <summary>
/// EF Core implementation of <see cref="IStoreOrderRepository"/>.
/// Maps between the domain <see cref="StoreOrderSnapshotData"/> DTO and the EF entity.
/// </summary>
public sealed class StoreOrderRepository(StoreOperationsDbContext dbContext) : IStoreOrderRepository
{
    public async Task UpsertSnapshotAsync(StoreOrderSnapshotData snapshot, CancellationToken cancellationToken = default)
    {
        var existing = await dbContext.StoreOrderSnapshots
            .FindAsync([snapshot.OrderId], cancellationToken);

        if (existing is null)
        {
            dbContext.StoreOrderSnapshots.Add(new StoreOrderSnapshot
            {
                OrderId      = snapshot.OrderId,
                CurrentState = snapshot.CurrentState,
                PriorityBand = snapshot.PriorityBand,
                IsRush       = snapshot.IsRush,
                IsAtRisk     = snapshot.IsAtRisk,
                CreatedAt    = snapshot.CreatedAt,
                UpdatedAt    = snapshot.UpdatedAt,
            });
        }
        else
        {
            // Optimistic-concurrency guard: reject stale writes.
            // The caller must supply the RowVersion obtained from the last GetSnapshotAsync call.
            // A mismatch means another writer has committed since the snapshot was read.
            if (existing.RowVersion != snapshot.RowVersion)
                throw new DbUpdateConcurrencyException(
                    $"Concurrency conflict on order {snapshot.OrderId}: " +
                    $"expected RowVersion {snapshot.RowVersion} but current is {existing.RowVersion}.");

            existing.CurrentState = snapshot.CurrentState;
            existing.PriorityBand = snapshot.PriorityBand;
            existing.IsRush       = snapshot.IsRush;
            existing.IsAtRisk     = snapshot.IsAtRisk;
            existing.UpdatedAt    = snapshot.UpdatedAt;
            existing.RowVersion++;  // advance token so the next caller must re-read
        }

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<StoreOrderSnapshotData?> GetSnapshotAsync(Guid orderId, CancellationToken cancellationToken = default)
    {
        var entity = await dbContext.StoreOrderSnapshots
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.OrderId == orderId, cancellationToken);

        return entity is null ? null : ToDto(entity);
    }

    public async Task AppendTransitionLogAsync(Guid orderId, string fromState, string toState, DateTimeOffset occurredAt, CancellationToken cancellationToken = default)
    {
        dbContext.OrderTransitionLogs.Add(new OrderTransitionLog
        {
            OrderId    = orderId,
            FromState  = fromState,
            ToState    = toState,
            OccurredAt = occurredAt,
        });
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    private static StoreOrderSnapshotData ToDto(StoreOrderSnapshot e) =>
        new(e.OrderId, e.CurrentState, e.PriorityBand, e.IsRush, e.IsAtRisk, e.CreatedAt, e.UpdatedAt, e.RowVersion);

    public async Task CompleteOrderAsync(
        StoreOrderSnapshotData snapshot,
        string outboxType,
        string outboxPayload,
        DateTimeOffset occurredAt,
        string? correlationId = null,
        CancellationToken cancellationToken = default)
    {
        // ── Snapshot upsert (tracked — no SaveChanges yet) ────────────────
        var existing = await dbContext.StoreOrderSnapshots
            .FindAsync([snapshot.OrderId], cancellationToken);

        if (existing is null)
        {
            dbContext.StoreOrderSnapshots.Add(new StoreOrderSnapshot
            {
                OrderId      = snapshot.OrderId,
                CurrentState = snapshot.CurrentState,
                PriorityBand = snapshot.PriorityBand,
                IsRush       = snapshot.IsRush,
                IsAtRisk     = snapshot.IsAtRisk,
                CreatedAt    = snapshot.CreatedAt,
                UpdatedAt    = snapshot.UpdatedAt,
            });
        }
        else
        {
            if (existing.RowVersion != snapshot.RowVersion)
                throw new DbUpdateConcurrencyException(
                    $"Concurrency conflict on order {snapshot.OrderId}: " +
                    $"expected RowVersion {snapshot.RowVersion} but current is {existing.RowVersion}.");

            existing.CurrentState = snapshot.CurrentState;
            existing.PriorityBand = snapshot.PriorityBand;
            existing.IsRush       = snapshot.IsRush;
            existing.IsAtRisk     = snapshot.IsAtRisk;
            existing.UpdatedAt    = snapshot.UpdatedAt;
            existing.RowVersion++;
        }

        // ── Transition log (tracked) ─────────────────────────────────────
        dbContext.OrderTransitionLogs.Add(new OrderTransitionLog
        {
            OrderId    = snapshot.OrderId,
            FromState  = "Ready",
            ToState    = "Completed",
            OccurredAt = occurredAt,
        });

        // ── Outbox message (tracked) ─────────────────────────────────────
        dbContext.OutboxMessages.Add(new OutboxMessage
        {
            Type          = outboxType,
            Payload       = outboxPayload,
            OccurredAt    = occurredAt,
            CorrelationId = correlationId,
        });

        // ── Single atomic save ───────────────────────────────────────────
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task CancelOrderAsync(
        StoreOrderSnapshotData snapshot,
        string fromState,
        string outboxType,
        string outboxPayload,
        DateTimeOffset occurredAt,
        string? correlationId = null,
        CancellationToken cancellationToken = default)
    {
        // ── Snapshot upsert (tracked — no SaveChanges yet) ────────────────
        var existing = await dbContext.StoreOrderSnapshots
            .FindAsync([snapshot.OrderId], cancellationToken);

        if (existing is null)
        {
            dbContext.StoreOrderSnapshots.Add(new StoreOrderSnapshot
            {
                OrderId      = snapshot.OrderId,
                CurrentState = snapshot.CurrentState,
                PriorityBand = snapshot.PriorityBand,
                IsRush       = snapshot.IsRush,
                IsAtRisk     = snapshot.IsAtRisk,
                CreatedAt    = snapshot.CreatedAt,
                UpdatedAt    = snapshot.UpdatedAt,
            });
        }
        else
        {
            if (existing.RowVersion != snapshot.RowVersion)
                throw new DbUpdateConcurrencyException(
                    $"Concurrency conflict on order {snapshot.OrderId}: " +
                    $"expected RowVersion {snapshot.RowVersion} but current is {existing.RowVersion}.");

            existing.CurrentState = snapshot.CurrentState;
            existing.PriorityBand = snapshot.PriorityBand;
            existing.IsRush       = snapshot.IsRush;
            existing.IsAtRisk     = snapshot.IsAtRisk;
            existing.UpdatedAt    = snapshot.UpdatedAt;
            existing.RowVersion++;
        }

        // ── Transition log (tracked) ─────────────────────────────────────
        dbContext.OrderTransitionLogs.Add(new OrderTransitionLog
        {
            OrderId    = snapshot.OrderId,
            FromState  = fromState,
            ToState    = "Cancelled",
            OccurredAt = occurredAt,
        });

        // ── Outbox message (tracked) ─────────────────────────────────────
        dbContext.OutboxMessages.Add(new OutboxMessage
        {
            Type          = outboxType,
            Payload       = outboxPayload,
            OccurredAt    = occurredAt,
            CorrelationId = correlationId,
        });

        // ── Single atomic save ───────────────────────────────────────────
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task MarkReadyAsync(
        StoreOrderSnapshotData snapshot,
        string outboxType,
        string outboxPayload,
        DateTimeOffset occurredAt,
        string? correlationId = null,
        CancellationToken cancellationToken = default)
    {
        // ── Snapshot upsert (tracked — no SaveChanges yet) ────────────────
        var existing = await dbContext.StoreOrderSnapshots
            .FindAsync([snapshot.OrderId], cancellationToken);

        if (existing is null)
        {
            dbContext.StoreOrderSnapshots.Add(new StoreOrderSnapshot
            {
                OrderId      = snapshot.OrderId,
                CurrentState = snapshot.CurrentState,
                PriorityBand = snapshot.PriorityBand,
                IsRush       = snapshot.IsRush,
                IsAtRisk     = snapshot.IsAtRisk,
                CreatedAt    = snapshot.CreatedAt,
                UpdatedAt    = snapshot.UpdatedAt,
            });
        }
        else
        {
            if (existing.RowVersion != snapshot.RowVersion)
                throw new DbUpdateConcurrencyException(
                    $"Concurrency conflict on order {snapshot.OrderId}: " +
                    $"expected RowVersion {snapshot.RowVersion} but current is {existing.RowVersion}.");

            existing.CurrentState = snapshot.CurrentState;
            existing.PriorityBand = snapshot.PriorityBand;
            existing.IsRush       = snapshot.IsRush;
            existing.IsAtRisk     = snapshot.IsAtRisk;
            existing.UpdatedAt    = snapshot.UpdatedAt;
            existing.RowVersion++;
        }

        // ── Transition log (tracked) ─────────────────────────────────────
        dbContext.OrderTransitionLogs.Add(new OrderTransitionLog
        {
            OrderId    = snapshot.OrderId,
            FromState  = "InProgress",
            ToState    = "Ready",
            OccurredAt = occurredAt,
        });

        // ── Outbox message (tracked) ─────────────────────────────────────
        dbContext.OutboxMessages.Add(new OutboxMessage
        {
            Type          = outboxType,
            Payload       = outboxPayload,
            OccurredAt    = occurredAt,
            CorrelationId = correlationId,
        });

        // ── Single atomic save ───────────────────────────────────────────
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}

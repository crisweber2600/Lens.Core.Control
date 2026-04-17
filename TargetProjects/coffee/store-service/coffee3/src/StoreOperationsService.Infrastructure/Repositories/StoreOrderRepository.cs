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
            existing.CurrentState = snapshot.CurrentState;
            existing.PriorityBand = snapshot.PriorityBand;
            existing.IsRush       = snapshot.IsRush;
            existing.IsAtRisk     = snapshot.IsAtRisk;
            existing.UpdatedAt    = snapshot.UpdatedAt;
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
        new(e.OrderId, e.CurrentState, e.PriorityBand, e.IsRush, e.IsAtRisk, e.CreatedAt, e.UpdatedAt);
}

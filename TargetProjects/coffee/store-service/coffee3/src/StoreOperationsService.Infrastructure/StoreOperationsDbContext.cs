using Microsoft.EntityFrameworkCore;
using StoreOperationsService.Infrastructure.Entities;

namespace StoreOperationsService.Infrastructure;

public sealed class StoreOperationsDbContext(DbContextOptions<StoreOperationsDbContext> options)
    : DbContext(options)
{
    public DbSet<StoreOrderSnapshot> StoreOrderSnapshots => Set<StoreOrderSnapshot>();
    public DbSet<OrderTransitionLog> OrderTransitionLogs => Set<OrderTransitionLog>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<StoreOrderSnapshot>(e =>
        {
            e.HasKey(x => x.OrderId);
            e.Property(x => x.CurrentState).HasMaxLength(50).IsRequired();
            e.Property(x => x.PriorityBand).HasMaxLength(50).IsRequired();
            e.Property(x => x.RowVersion).IsRowVersion();
            e.HasIndex(x => x.CurrentState);
        });

        modelBuilder.Entity<OrderTransitionLog>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).UseIdentityColumn();
            e.Property(x => x.FromState).HasMaxLength(50).IsRequired();
            e.Property(x => x.ToState).HasMaxLength(50).IsRequired();
            e.HasIndex(x => x.OrderId);
            e.HasIndex(x => x.OccurredAt);
        });
    }
}

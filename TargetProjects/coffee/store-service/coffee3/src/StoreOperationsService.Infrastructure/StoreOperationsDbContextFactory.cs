using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace StoreOperationsService.Infrastructure;

/// <summary>
/// Used by EF Core design-time tooling (dotnet ef migrations).
/// Connects to a local SQL Server dev instance when running migrations manually.
/// </summary>
public sealed class StoreOperationsDbContextFactory
    : IDesignTimeDbContextFactory<StoreOperationsDbContext>
{
    public StoreOperationsDbContext CreateDbContext(string[] args)
    {
        var options = new DbContextOptionsBuilder<StoreOperationsDbContext>()
            .UseSqlServer(
                "Server=(localdb)\\mssqllocaldb;Database=StoreOperationsDb;Trusted_Connection=True;",
                sql => sql.MigrationsAssembly(typeof(StoreOperationsDbContext).Assembly.FullName))
            .Options;

        return new StoreOperationsDbContext(options);
    }
}

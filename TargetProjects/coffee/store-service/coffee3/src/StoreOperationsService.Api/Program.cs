using Microsoft.EntityFrameworkCore;
using StoreOperationsService.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHealthChecks();

builder.Services.AddDbContext<StoreOperationsDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("StoreOperationsDb")
            ?? "Server=(localdb)\\mssqllocaldb;Database=StoreOperationsDb;Trusted_Connection=True;",
        sql => sql.MigrationsAssembly(typeof(StoreOperationsDbContext).Assembly.FullName)));

var app = builder.Build();

app.MapHealthChecks("/health");

app.Run();
public partial class Program { }

public partial class Program { }

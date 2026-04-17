using Microsoft.EntityFrameworkCore;
using StoreOperationsService.Domain.Messaging;
using StoreOperationsService.Domain.Queue;
using StoreOperationsService.Domain.Repositories;
using StoreOperationsService.Infrastructure;
using StoreOperationsService.Infrastructure.Messaging;
using StoreOperationsService.Infrastructure.Queue;
using StoreOperationsService.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHealthChecks();

builder.Services.AddDbContext<StoreOperationsDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("StoreOperationsDb")
            ?? "Server=(localdb)\\mssqllocaldb;Database=StoreOperationsDb;Trusted_Connection=True;",
        sql => sql.MigrationsAssembly(typeof(StoreOperationsDbContext).Assembly.FullName)));

builder.Services.AddSingleton<IEventBusAdapter, InMemoryEventBusAdapter>();
builder.Services.AddSingleton<IRabbitMqConsumerAdapter, InMemoryConsumerAdapter>();
builder.Services.AddScoped<IStoreOrderRepository, StoreOrderRepository>();
builder.Services.AddScoped<IOutboxRepository, OutboxRepository>();
builder.Services.AddQueueReadModel();
builder.Services.AddScoped<IOrderIntakeConsumer, OrderIntakeConsumer>();
builder.Services.AddHostedService<OutboxPublisherService>();
builder.Services.AddHostedService<OrderIntakeSubscriber>();

var app = builder.Build();

app.MapHealthChecks("/health");

app.Run();
public partial class Program { }

public partial class Program { }

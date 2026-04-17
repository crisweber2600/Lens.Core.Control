---
doc_type: aspire-plan
feature: coffee-store-service-mvp
status: draft
updated_at: '2026-05-07T00:00:00Z'
---

# Aspire Plan — StoreOperationsService (coffee-store-service-mvp)

> **Specification for story S5.2** — this document defines the shared .NET Aspire AppHost that S5.2 must build before any coffee service can run locally. The Coffee Domain Aspire Mandate (constitution hard gate) requires all coffee services to use this shared AppHost; no service may be run outside it, including for MVP.

---

## AppHost Project

**Project name:** `Coffee.AppHost`  
**Project path:** `TargetProjects/coffee/Coffee.AppHost/Coffee.AppHost.csproj`  
**Solution file:** `TargetProjects/coffee/Coffee.sln`

### Proposed solution structure

```
TargetProjects/coffee/
├── Coffee.sln
├── Coffee.AppHost/
│   ├── Coffee.AppHost.csproj        # Aspire AppHost — references all service projects
│   └── Program.cs                   # Orchestration entry point
├── customer-service/
│   └── coffee1/                     # CustomerService project (stub; provisioned separately)
├── order-service/
│   └── coffee2/                     # OrderService project (stub; provisioned separately)
└── store-service/
    └── <tbd>/                       # StoreOperationsService project (to be provisioned in S5.2)
```

`Coffee.AppHost` is a standard .NET Aspire `Microsoft.NET.Sdk` project with a `<IsAspireHost>true</IsAspireHost>` property. It references each service project directly so Aspire can launch them in process during local development.

The AppHost does **not** contain business logic. It is purely an orchestration project: it wires resources, injects configuration, and monitors health.

---

## Service Registration

The following snippet shows the complete registration block for `StoreOperationsService` inside `Coffee.AppHost/Program.cs`. Resources are declared once and referenced by name.

```csharp
var builder = DistributedApplication.CreateBuilder(args);

// ── Shared infrastructure ────────────────────────────────────────────────────
var sql = builder.AddSqlServer("coffee-sql");
var storeDb = sql.AddDatabase("storeops-db");

var messaging = builder.AddRabbitMQ("coffee-messaging");

// ── StoreOperationsService ───────────────────────────────────────────────────
builder.AddProject<Projects.StoreOperationsService>("coffee-store-service")
       .WithReference(storeDb)
       .WithReference(messaging)
       .WithHttpHealthCheck(path: "/health");

// ── Other coffee services (registered here so they share infrastructure) ─────
builder.AddProject<Projects.CustomerService>("coffee-customer-service")
       .WithReference(sql.AddDatabase("customerops-db"))
       .WithReference(messaging)
       .WithHttpHealthCheck(path: "/health");

builder.AddProject<Projects.OrderService>("coffee-order-service")
       .WithReference(sql.AddDatabase("orderops-db"))
       .WithReference(messaging)
       .WithHttpHealthCheck(path: "/health");

await builder.Build().RunAsync();
```

Key points:
- `Projects.StoreOperationsService` is the generated type alias for the `store-service` csproj; the project reference in `Coffee.AppHost.csproj` creates it automatically.
- `"coffee-store-service"` is the logical Aspire resource name used in the dashboard and in service-discovery lookups.
- `WithReference(storeDb)` and `WithReference(messaging)` cause Aspire to inject the relevant connection strings as environment variables at startup; no manual configuration is required.
- `WithHttpHealthCheck(path: "/health")` tells Aspire which HTTP endpoint to poll; the dashboard turns the tile green only when the endpoint returns `200`.

---

## Resource Bindings

All bindings are Aspire-managed. Aspire resolves connection strings at runtime and injects them via `IConfiguration`; services never hardcode or read them from `appsettings.json`.

| Resource Name       | Aspire Type      | Aspire Method                         | Notes                                                                                          |
|---------------------|------------------|---------------------------------------|------------------------------------------------------------------------------------------------|
| `coffee-sql`        | SQL Server       | `AddSqlServer("coffee-sql")`          | Shared SQL Server instance. Aspire starts a containerised SQL Server for local dev.            |
| `storeops-db`       | SQL Database     | `sql.AddDatabase("storeops-db")`      | Database scoped to StoreOperationsService. Hosts `store_orders`, `store_order_transitions`, `store_queue_projection`, `processed_source_messages`, `outbox_events`. |
| `coffee-messaging`  | RabbitMQ         | `AddRabbitMQ("coffee-messaging")`     | Shared RabbitMQ broker. Aspire starts a containerised RabbitMQ for local dev. Used for Transactional Outbox event publication and upstream/downstream message consumption. |

> **Secrets:** SQL Server password and RabbitMQ credentials are managed via Aspire's user-secrets / generated environment variables. They are **never** committed to source control.

---

## Health Check

### ASP.NET Core registration (StoreOperationsService)

In the service's `Program.cs` (or `Startup.cs`):

```csharp
builder.Services.AddHealthChecks()
    .AddSqlServer(
        connectionStringFactory: sp =>
            sp.GetRequiredService<IConfiguration>().GetConnectionString("storeops-db")
            ?? throw new InvalidOperationException("storeops-db connection string not found"),
        name: "storeops-db",
        failureStatus: HealthStatus.Unhealthy,
        tags: ["db", "sql"])
    .AddRabbitMQ(
        rabbitConnectionStringFactory: sp =>
            sp.GetRequiredService<IConfiguration>().GetConnectionString("coffee-messaging")
            ?? throw new InvalidOperationException("coffee-messaging connection string not found"),
        name: "coffee-messaging",
        failureStatus: HealthStatus.Degraded,
        tags: ["messaging"]);

// …

app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});
```

Use `AspNetCore.HealthChecks.SqlServer` and `AspNetCore.HealthChecks.Rabbitmq` NuGet packages for the provider-specific checks.

### Aspire monitoring hook

The `WithHttpHealthCheck(path: "/health")` call in the AppHost (shown in [Service Registration](#service-registration)) polls this endpoint. Aspire retries on startup until the endpoint returns `200`; the dashboard shows:

- **Starting** — service process has launched, health endpoint not yet reachable.
- **Healthy** — `/health` returned `200` with all checks passing.
- **Unhealthy** — `/health` returned non-`200` or a check reported `Unhealthy`.

No additional Aspire configuration is required for health monitoring beyond the single `WithHttpHealthCheck` line.

---

## Connection Strings

Aspire injects all connection strings as environment variables using the standard .NET configuration key format. Services read them via `IConfiguration` — no hardcoded values anywhere in `appsettings.json` or code.

### SQL Server — `storeops-db`

Aspire injects the database connection string under the key:

```
ConnectionStrings__storeops-db
```

Which maps to `IConfiguration["ConnectionStrings:storeops-db"]` in C#. The service's `DbContext` registration:

```csharp
builder.Services.AddDbContext<StoreOpsDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("storeops-db")
        ?? throw new InvalidOperationException("storeops-db connection string missing")));
```

Aspire constructs the full connection string (server, port, credentials) from the resource definition at AppHost startup. The StoreOperationsService project sees a standard ADO.NET connection string with no awareness of how it was generated.

### RabbitMQ — `coffee-messaging`

Aspire injects the AMQP URI under the key:

```
ConnectionStrings__coffee-messaging
```

Which maps to `IConfiguration["ConnectionStrings:coffee-messaging"]`. Registration for MassTransit (recommended for outbox):

```csharp
builder.Services.AddMassTransit(x =>
{
    x.UsingRabbitMq((ctx, cfg) =>
    {
        var connectionString = ctx.GetRequiredService<IConfiguration>()
            .GetConnectionString("coffee-messaging")
            ?? throw new InvalidOperationException("coffee-messaging connection string missing");

        cfg.Host(new Uri(connectionString));
        cfg.ConfigureEndpoints(ctx);
    });
});
```

### Summary table

| Resource          | IConfiguration key                       | Format injected by Aspire      | Hardcoded in appsettings.json? |
|-------------------|------------------------------------------|--------------------------------|-------------------------------|
| `storeops-db`     | `ConnectionStrings:storeops-db`          | ADO.NET SQL Server string      | No                            |
| `coffee-messaging`| `ConnectionStrings:coffee-messaging`     | AMQP URI (`amqp://...`)        | No                            |

---

## Local Dev Validation

These steps verify that `StoreOperationsService` is healthy inside the Aspire AppHost before Sprint 1 integration work begins. Run from the repository root (`TargetProjects/coffee/`).

1. **Ensure Docker (or Podman) is running** — Aspire starts containerised SQL Server and RabbitMQ. Confirm with:
   ```bash
   docker info
   ```

2. **Restore and build the solution:**
   ```bash
   dotnet restore Coffee.sln
   dotnet build Coffee.sln --no-restore
   ```
   Expect zero errors. If `Projects.StoreOperationsService` is unresolved, ensure the `<ProjectReference>` to the store-service csproj is present in `Coffee.AppHost.csproj`.

3. **Start the AppHost:**
   ```bash
   dotnet run --project Coffee.AppHost/Coffee.AppHost.csproj
   ```
   Aspire prints the dashboard URL (default `https://localhost:15888`). Wait for the log line:
   ```
   Login to the dashboard at https://localhost:15888/login?t=<token>
   ```

4. **Open the Aspire dashboard** at the printed URL. On the **Resources** tab, confirm all tiles are present:
   - `coffee-sql` — should show **Running**
   - `coffee-messaging` — should show **Running**
   - `coffee-store-service` — should progress from **Starting** → **Healthy**

5. **Verify the health endpoint directly:**
   ```bash
   curl -s https://localhost:<store-service-port>/health | jq .
   ```
   Expected response status: `200`. Expected body: JSON with `"status": "Healthy"` and entries for `storeops-db` and `coffee-messaging` both reporting `"Healthy"` or `"Degraded"` (not `"Unhealthy"`).

   The port is shown in the Aspire dashboard on the `coffee-store-service` row under **Endpoints**.

6. **Confirm database connectivity** — check Aspire dashboard structured logs for `coffee-store-service`. Look for EF Core migration log lines or the absence of `SqlException` errors on startup.

7. **Confirm message broker connectivity** — look for MassTransit startup log lines such as `Bus started` with no connection-refused errors.

8. **Stop the AppHost** with `Ctrl+C`. Confirm containers stop cleanly (Aspire tears down SQL Server and RabbitMQ containers automatically).

> **Pass criteria:** Steps 4 and 5 both show `coffee-store-service` as Healthy with no dependency errors. This gate must be met before any Sprint 1 implementation story begins coding against the service.

---

*This document is the specification for story **S5.2 — Provision shared Coffee.AppHost**. S5.2 owns creating the `Coffee.AppHost` project, adding it to `Coffee.sln`, and registering all coffee services so this validation checklist passes.*

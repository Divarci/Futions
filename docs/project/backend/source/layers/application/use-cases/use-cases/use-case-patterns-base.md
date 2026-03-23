# UseCase Patterns — Base

## Role of the UseCase Layer

UseCase classes sit at the top of the application layer, directly below the adapters
(controllers). Each UseCase is responsible for exactly one entity or aggregate boundary.
Its job is to:

1. Wrap write operations inside a database transaction (`ITransactionalUnitOfWork`).
2. Delegate domain work to the corresponding service.
3. Record an audit trail via `IAuditLogService` after every successful mutation.
4. Serve read operations directly from cache via `ICacheProvider`, bypassing the service
   when a hot entry exists.

UseCases never contain domain logic. They orchestrate — calling services and
infrastructure abstractions in the correct order while leaving all business rules to the
service and domain layers.

---

## Anatomy

**Root file** — `{Entity}UseCase.cs`

Contains only the class declaration, the primary constructor, readonly field assignments,
and the `_timeout` constant. No methods.

```csharp
using Core.Library.Contracts.Caching;
using Core.Library.Contracts.Persistence;
using Microsoft.Extensions.Logging;

namespace App.UseCases.UseCases.{Module}.{Entities};

internal sealed partial class {Entity}UseCase(
    I{Entity}Service {entity}Service,
    IAuditLogService auditLogService,
    ICacheProvider cacheProvider,
    ITransactionalUnitOfWork unitOfWork,
    ILogger<{Entity}UseCase> logger) : I{Entity}UseCase
{
    private readonly I{Entity}Service _{entity}Service = {entity}Service;
    private readonly IAuditLogService _auditLogService = auditLogService;
    private readonly ICacheProvider _cacheProvider = cacheProvider;
    private readonly ITransactionalUnitOfWork _unitOfWork = unitOfWork;
    private readonly ILogger<{Entity}UseCase> _logger = logger;

    private readonly TimeSpan _timeout = TimeSpan.FromHours(1);
}
```

Five dependencies are always present: the entity's own service, the audit log service,
the cache provider, the unit of work, and a typed logger. The `_timeout` field is the
single non-injected state — it controls how long cache entries live for this entity.

The class is `internal sealed partial` — `internal` because UseCases are not exposed
outside the assembly, `sealed` to prevent inheritance, `partial` to allow
operation-specific files.

The `I{Entity}UseCase` interface is defined in
`Api/Source/Core/Core.Domain/Entities/{Module}/{Entities}/Interfaces/`.
This keeps the abstraction in the domain layer and avoids circular references.

**Operation files** — one per operation

Each operation (Create, Update, Delete, Single, Collection) lives in its own partial
file. See the dedicated pattern documents for each.

---

## DI Registration

**File:** `Api/Source/Application/App.UseCases/ServiceRegistrar.cs`

Every UseCase is registered as `AddScoped<I{Entity}UseCase, {Entity}UseCase>()`.
Scoped lifetime aligns with the HTTP request and with the persistence context, which is
also scoped. UseCases must never be registered as singletons.

---

## Critical Rules

- UseCases never call domain or repository methods directly — all domain work goes
  through the service.
- Every write operation must be wrapped in `ExecuteTransactionAsync`. Never call
  `SaveChangesAsync` directly.
- Audit log creation failure is **non-fatal**. Log a warning with a trace ID and let the
  write succeed.
- Cache key construction is always delegated to `CacheKeyHelper` in the `Helpers/`
  folder — never build raw strings inside UseCase methods.
- The `_timeout` field is the single source of truth for cache TTL per UseCase class.
  Never hardcode `TimeSpan` values in operation files.

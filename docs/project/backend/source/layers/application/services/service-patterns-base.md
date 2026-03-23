# Service Patterns — Base

## Role of the Service Layer

Service classes sit between the repository layer and the UseCase layer. Each service is
responsible for exactly one entity. Its job is to:

1. Validate inputs by calling domain factory or update methods.
2. Call the entity's own business logic (domain methods on the aggregate).
3. Delegate persistence to the repository (change tracker only — never `SaveChangesAsync`).
4. Invalidate affected cache entries via `ICacheInvalidationService`.

Services never start transactions, never call `SaveChangesAsync`, and never orchestrate
multiple entities together. Orchestration is the UseCase layer's responsibility.

---

## Anatomy

**Root file** — `{Entity}Service.cs`

Contains only the class declaration, the primary constructor, and readonly field
assignments. No methods.

```csharp
using Core.Domain.Entities.{Module}.{Entities}.Interfaces;
using Core.Library.Contracts.Caching;

namespace App.Services.Features.{Module}.{Entities};

internal sealed partial class {Entity}Service(
    I{Entity}Repository {entity}Repository,
    ICacheInvalidationService cacheInvalidationService) : I{Entity}Service
{
    private readonly I{Entity}Repository _{entity}Repository = {entity}Repository;
    private readonly ICacheInvalidationService _cacheInvalidationService = cacheInvalidationService;
}
```

Dependencies are always declared as `private readonly` fields. The class is `internal
sealed partial` — `internal` because services are not exposed outside the assembly,
`sealed` to prevent inheritance, `partial` to allow operation-specific files.

When a service needs to perform pre-condition checks against a related entity (e.g.
checking for dependent records before deletion), inject the related repository as an
additional constructor parameter. Real example:
`Api/Source/Application/App.Services/Organisations/Companies/CompanyService.cs`

**Operation files** — one per operation

Each operation (Create, Update, Delete, Single, Collection) lives in its own partial
file. See the dedicated pattern documents for each.

---

## DI Registration

**File:** `Api/Source/Application/App.Services/ServiceRegistrar.cs`

Every service is registered as `AddScoped<I{Entity}Service, {Entity}Service>()`. Scoped
lifetime matches the request lifetime and aligns with the persistence context, which is
also scoped. Services must never be registered as singletons.

---

## Critical Rules

- A service handles **one entity** type only. Cross-entity orchestration belongs in the UseCase layer.
- Services never call `SaveChangesAsync`. All persistence is committed by the Unit of Work.
- Services never begin or manage transactions.
- Cache invalidation must happen **after** the repository call succeeds, not before.
- The `cacheKeyBuilder` delegate is always received from the caller (UseCase) — services never construct cache keys themselves.

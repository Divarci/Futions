# Repository Patterns — Generic Repository

Documents the three generic base classes that form the repository hierarchy.
Concrete repositories never use these directly — they inherit from either
`TenantedRepository<TEntity>` or `GlobalRepository<TEntity>`.

---

## Design

The hierarchy has three levels, each building on the one below.

**`BaseRepository<TEntity>`**
`Api/Source/Infrastructure/Infra.Persistence/Repositories/Generics/BaseRepository.cs`

Abstract base for all repositories. Holds the `AppDbContext` reference and implements
`IBaseRepository<TEntity>` — the write-only contract. Write operations (`CreateAsync`,
`CreateRangeAsync`, `Update`, `UpdateRange`, `Delete`, `DeleteRange`) interact with the
EF Core change tracker only. None of them call `SaveChangesAsync`; persistence is
committed exclusively by the Unit of Work.

Create operations are async because they call `AddAsync` / `AddRangeAsync`. Update and
Delete operations are synchronous because EF change-tracking is in-memory. A `null`
entity or collection passed to any write method is treated as a programmer error and
returns `HttpStatusCode.InternalServerError`. Each method is implemented in its own
partial file:

- `Api/Source/Infrastructure/Infra.Persistence/Repositories/Generics/BaseRepository.Create.cs`
- `Api/Source/Infrastructure/Infra.Persistence/Repositories/Generics/BaseRepository.Update.cs`
- `Api/Source/Infrastructure/Infra.Persistence/Repositories/Generics/BaseRepository.Delete.cs`

**`TenantedRepository<TEntity>`**
`Api/Source/Infrastructure/Infra.Persistence/Repositories/Generics/TenantedRepository.cs`

Extends `BaseRepository` for entities that implement `IHaveTenant`. Every read query is
automatically scoped by `tenantId` — `GetByIdAsync(id, tenantId)`, `CountAsync(tenantId)`,
and `ExistsAsync(id, tenantId)` all filter on `TenantId` before any other predicate.
`SingleOrDefaultAsync` is used for single-entity lookups; a missing match returns
`HttpStatusCode.NotFound`. Read operations are split across partial files:

- `Api/Source/Infrastructure/Infra.Persistence/Repositories/Generics/TenantedRepository.Single.cs`

**`GlobalRepository<TEntity>`**
`Api/Source/Infrastructure/Infra.Persistence/Repositories/Generics/GlobalRepository.cs`

Extends `BaseRepository` for entities that do not implement `IHaveTenant` (system-level
or join entities). Read operations (`GetByIdAsync(id)`, `CountAsync()`, `ExistsAsync(id)`)
carry no tenant filter. Otherwise the semantics are identical to `TenantedRepository`.

---

## Usage in Concrete Repositories

Concrete repositories inherit from exactly one of the two intermediate classes and add
domain-specific query methods in dedicated partial files. Real examples:

- `Api/Source/Infrastructure/Infra.Persistence/Repositories/Organisations/People/PersonRepository.cs` — inherits `TenantedRepository<Person>`
- `Api/Source/Infrastructure/Infra.Persistence/Repositories/Organisations/Companies/CompanyRepository.cs` — inherits `TenantedRepository<Company>`

When a concrete repository needs a query beyond the built-in three, a new named method is
added in a new partial file. The base `GetByIdAsync`, `CountAsync`, and `ExistsAsync`
methods are never shadowed or overridden.

---

## Critical Rules

- `TenantedRepository` is for entities implementing `IHaveTenant`. `GlobalRepository` is
  for all others. Never inherit from both in the same repository.
- The built-in `GetByIdAsync`, `CountAsync`, and `ExistsAsync` cover the common case.
  When additional filter predicates are needed, add a **new named method** in a dedicated
  partial file — do not shadow or override the base method.
- Write operations (`Create`, `Update`, `Delete`) never call `SaveChangesAsync`.
  Persistence is committed exclusively by the Unit of Work.
- A `null` entity or collection passed to any write method is a programmer error and
  returns `HttpStatusCode.InternalServerError`.

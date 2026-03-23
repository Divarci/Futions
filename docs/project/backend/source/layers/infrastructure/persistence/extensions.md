# IQueryable Extensions

`IQueryableExtensions` provides three fluent extension methods that keep repository query
composition clean and conditional-branch-free.

**File:** `Api/Source/Infrastructure/Infra.Persistence/Extensions/IQueryableExtentions.cs`

---

## Design

All three methods follow the same contract: they accept a `bool condition` and only apply
their behaviour when that condition is `true`. When `false`, the original `IQueryable<T>`
is returned unchanged. This lets repository methods chain multiple optional predicates in
a single expression without `if` blocks.

**`WhereIf`**

Applies a filter predicate only when the condition is met. The primary use case is
optional user-supplied filter strings — the caller passes
`!string.IsNullOrWhiteSpace(filter)` as the condition, so an empty filter produces no
SQL `WHERE` clause at all.

**`OrderByIf`**

Resolves the `sortBy` string to an actual property name via reflection
(`BindingFlags.IgnoreCase`), then calls either `OrderBy` or `OrderByDescending` depending
on `isAscending`. If the provided property name does not match any public property, it
falls back to `Id`. This means malformed sort parameters never throw — they silently
default to a stable sort.

**`IncludeIf`**

Applies an `Include` navigation expression only when the condition is true. Used when a
navigation property should only be eagerly loaded for certain query paths.

---

## Usage in Repositories

Every paginated repository method uses `WhereIf` + `OrderByIf`. Real examples:

- `Api/Source/Infrastructure/Infra.Persistence/Repositories/Organisations/Companies/CompanyRepository.Collection.cs` — filters by `Name` with `EF.Functions.Like`, sorts with `OrderByIf`.
- `Api/Source/Infrastructure/Infra.Persistence/Repositories/Organisations/People/PersonRepository.Collection.cs` — filters across two value-object properties (`FirstName`, `LastName`); `WhereIf` wraps the multi-column `Like` block.
- `Api/Source/Infrastructure/Infra.Persistence/Repositories/Organisations/CompanyPeople/CompanyPersonRepository.Collection.cs` — combines a multi-table `Where` base clause with `WhereIf` for optional title filtering.
- `Api/Source/Infrastructure/Infra.Persistence/Repositories/System/AuditLogs/AuditLogRepository.Collection.cs` — same pattern applied to a system entity.

---

## How to Scale

Adding a new conditional query behaviour follows the same pattern: a new static extension
method on `IQueryable<T>` in the same file, accepting a `bool condition` as its first
parameter after `this IQueryable<T>`. No existing methods need to change.

Examples of extensions that would fit here:

- `WhereIfNotNull` — apply a predicate only when a nullable value is provided.
- `IncludeIfNavigation` — conditionally eager-load based on a feature flag.
- `AsNoTrackingIf` — skip change-tracking for read-only call paths.

The file is internal to `Infra.Persistence` and has no public contract — callers inside
the infrastructure layer use it directly via the `using` directive. No interface or DI
registration is required.

---

## Critical Rules

- Every method must short-circuit (return the original query unchanged) when the
  condition is `false`. Never mutate the query unconditionally.
- `OrderByIf` falls back to `Id` when the property name cannot be resolved. Do not
  remove this fallback — it prevents runtime exceptions from untrusted sort parameters.
- These extensions are for the persistence layer only. Do not reference them from
  Application or Domain layers.

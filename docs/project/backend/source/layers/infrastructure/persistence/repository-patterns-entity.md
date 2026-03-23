# Repository Patterns — Entity Repository

Describes how concrete repositories are structured. Every concrete repository is a
`internal sealed partial` class split across multiple files — one file per concern.
All custom query methods follow the same file-naming convention.

> Reference implementation: `CompanyPersonRepository` (all partials).

---

## Pattern

### {Entity}Repository.cs — root declaration

Contains only the class declaration and primary constructor. No methods.

```csharp
internal sealed partial class {Entity}Repository(
    AppDbContext context) : GlobalRepository<{Entity}>(context), I{Entity}Repository
{
}
```

For a tenanted entity (implements `IHaveTenant`), inherit from `TenantedRepository` instead:

```csharp
internal sealed partial class {Entity}Repository(
    AppDbContext context) : TenantedRepository<{Entity}>(context), I{Entity}Repository
{
}
```

---

### {Entity}Repository.Collection.cs

Paginated list query. Always apply `AsNoTracking`. Scope by tenant and deleted state.

```csharp
internal sealed partial class {Entity}Repository
{
    public async Task<Result<{Entity}[]>> GetPaginated{Entities}Async(
        Guid tenantId,
        Guid parentId,
        int page,
        int pageSize,
        string sortBy,
        bool isAscending,
        string? filter,
        CancellationToken cancellationToken = default)
    {
        var query = _context.Set<{Entity}>()
            .AsNoTracking()
            .Where(x =>
                x.ParentA.TenantId == tenantId &&
                x.ParentAId == parentId &&
                !x.ParentA.IsDeleted)
            .WhereIf(!string.IsNullOrWhiteSpace(filter), x => EF.Functions.Like(x.Name, $"%{filter}%"))
            .OrderByIf(isAscending, sortBy);

        {Entity}[] entities = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToArrayAsync(cancellationToken);

        return Result<{Entity}[]>.Success(
            message: "{Entities} retrieved successfully.",
            data: entities,
            statusCode: HttpStatusCode.OK);
    }
}
```

For a directly tenanted entity (owns `TenantId`), the `Where` simplifies to:

```csharp
.Where(x => x.TenantId == tenantId && !x.IsDeleted)
```

---

### {Entity}Repository.Single.cs

Custom single-entity query when the built-in `GetByIdAsync` from the generic base does
not cover the required filter predicates.

```csharp
internal sealed partial class {Entity}Repository
{
    public async Task<Result<{Entity}>> Get{Entity}ByIdAsync(
        Guid tenantId,
        Guid parentId,
        Guid id,
        CancellationToken cancellationToken = default)
    {
        {Entity}? entity = await _context.Set<{Entity}>()
            .AsNoTracking()
            .SingleOrDefaultAsync(x =>
                x.Id == id &&
                x.ParentA.TenantId == tenantId &&
                x.ParentAId == parentId &&
                !x.ParentA.IsDeleted,
                cancellationToken);

        if (entity is null)
            return Result<{Entity}>.Failure(
                message: "{Entity} not found.",
                statusCode: HttpStatusCode.NotFound);

        return Result<{Entity}>.Success(
            message: "{Entity} retrieved successfully.",
            data: entity,
            statusCode: HttpStatusCode.OK);
    }
}
```

---

### {Entity}Repository.Count.cs

Custom count query when the built-in `CountAsync` from the generic base does not cover
the required filter predicates.

```csharp
internal sealed partial class {Entity}Repository
{
    public async Task<Result<int>> Count{Entities}Async(
        Guid tenantId,
        Guid parentId,
        CancellationToken cancellationToken = default)
    {
        int count = await _context.Set<{Entity}>()
            .AsNoTracking()
            .CountAsync(x =>
                x.ParentA.TenantId == tenantId &&
                x.ParentAId == parentId &&
                !x.ParentA.IsDeleted,
                cancellationToken);

        return Result<int>.Success(
            message: "Count retrieved successfully.",
            data: count,
            statusCode: HttpStatusCode.OK);
    }
}
```

---

### {Entity}Repository.Exist.cs

Custom existence queries when the built-in `ExistsAsync` from the generic base does not
cover the required filter predicates. Multiple methods can live in the same file.

```csharp
internal sealed partial class {Entity}Repository
{
    public async Task<Result<bool>> Has{Children}Async(
        Guid tenantId,
        Guid parentId,
        CancellationToken cancellationToken = default)
    {
        bool has = await _context.Set<{Entity}>()
            .AsNoTracking()
            .AnyAsync(x =>
                x.ParentAId == parentId &&
                x.ParentA.TenantId == tenantId &&
                !x.ParentA.IsDeleted,
                cancellationToken);

        return Result<bool>.Success(
            message: has ? "Parent has associated {children}." : "Parent has no associated {children}.",
            data: has,
            statusCode: HttpStatusCode.OK);
    }

    public async Task<Result<bool>> BelongsToTenantAsync(
        Guid tenantId,
        Guid parentId,
        Guid id,
        CancellationToken cancellationToken = default)
    {
        bool belongs = await _context.Set<{Entity}>()
            .AsNoTracking()
            .AnyAsync(x =>
                x.Id == id &&
                x.ParentA.TenantId == tenantId &&
                x.ParentAId == parentId &&
                !x.ParentA.IsDeleted,
                cancellationToken);

        return Result<bool>.Success(
            message: belongs ? "{Entity} belongs to the tenant." : "{Entity} does not belong to the tenant.",
            data: belongs,
            statusCode: HttpStatusCode.OK);
    }
}
```

---

## Critical Rules

- Concrete repositories are `internal sealed partial`. Never `public`.
- The root file (`{Entity}Repository.cs`) contains only the class declaration and
  constructor. All methods go in separate partial files.
- Every concrete repository implements exactly one domain interface (`I{Entity}Repository`).
- Choose `TenantedRepository` for entities implementing `IHaveTenant`, `GlobalRepository`
  for all others. Never mix both in the same repository.
- `_context` is the only field. Repositories must not hold any other state.
- Always call `AsNoTracking()` on all read queries. Never track entities in collection or
  existence queries.
- Use `SingleOrDefaultAsync` in single-entity queries, never `FirstOrDefaultAsync`.
  Duplicate IDs must surface as an exception, not silently return the first match.
- `WhereIf` and `OrderByIf` are extension methods from `Infra.Persistence`. Never use
  inline `if` blocks to conditionally build queries.
- Collection queries always return `{Entity}[]`, never `IEnumerable` or `List<T>`.
- Never call `SaveChangesAsync` inside a repository. Persistence is committed by the
  Unit of Work (`ITransactionalUnitOfWork.ExecuteTransactionAsync`).

> **Note — generic base overloads:** `TenantedRepository` and `GlobalRepository` already
> provide `GetByIdAsync`, `CountAsync`, and `ExistsAsync` with standard signatures. Only
> create a custom partial file for one of these operations when the built-in signature is
> insufficient (e.g. extra parent-ID filter, navigation-based tenant scoping). Custom
> methods use a **different name** — do not shadow or hide the base method.

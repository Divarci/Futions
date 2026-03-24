# Domain Patterns — Entity

Defines the complete structure an entity must follow from top to bottom. Two patterns are
covered: a standard business entity and a reference-data (seed) entity.

---

## Pattern 1 — Standard Business Entity

A business entity can opt into soft-delete and/or multi-tenancy by implementing
`IHaveSoftDelete` and `IHaveTenant` from `Core.Library`. The example below implements both.

### Folder Structure

```
Entities/[Module]/{Entities}/
├── {Entity}.cs
├── {Entity}.Validate.cs
├── DomainEvents/
│   ├── {Entity}CreatedDomainEvent.cs
│   ├── {Entity}TitleUpdatedDomainEvent.cs
│   └── {Entity}DeletedDomainEvent.cs
├── Models/
│   ├── {Entity}CreateModel.cs
│   └── {Entity}UpdateModel.cs
└── Interfaces/
    ├── I{Entity}Repository.cs
    ├── I{Entity}Service.cs
    └── I{Entity}UseCase.cs
```

### {Entity}.cs

```csharp
using Core.Library.Abstractions;
using Core.Library.Abstractions.Interfaces;
using Core.Library.Exceptions;
using Core.Library.ResultPattern;
using System.Net;

public partial class {Entity} : BaseEntity, IHaveSoftDelete, IHaveTenant
{
    // Constructors
    private {Entity}() { }

    private {Entity}(Guid tenantId, string title)
    {
        TenantId = tenantId;
        Title = title;
    }

    // Properties
    public string Title { get; private set; } = default!;

    // IHaveSoftDelete
    public bool IsDeleted { get; private set; }

    // IHaveTenant
    public Guid TenantId { get; private set; }

    // Methods
    public static Result<{Entity}> Create({Entity}CreateModel model)
    {
        if (model is null)
            throw new {Solution}Exception(
                assemblyName: "Core.Domain",
                className: nameof({Entity}),
                methodName: nameof(Create),
                message: "{Entity} create model cannot be null.");

        {Entity} {Entity} = new(model.TenantId, model.Title);

        Result validationResult = Validate({Entity});

        if (validationResult.IsFailure)
            return Result<{Entity}>.Failure(
                message: validationResult.Message,
                errorDetails: validationResult.ErrorDetails!,
                statusCode: validationResult.StatusCode);

        {Entity}.Raise(new {Entity}CreatedDomainEvent({Entity}.Id));

        return Result<{Entity}>.Success(
            message: "{Entity} created successfully.",
            data: {Entity},
            statusCode: HttpStatusCode.OK);
    }

    // Business Methods
    public Result SoftDelete()
    {
        if (IsDeleted)
            return Result.Failure(
                message: "{Entity} is already deleted.",
                statusCode: HttpStatusCode.BadRequest);

        IsDeleted = true;

        Raise(new {Entity}DeletedDomainEvent(Id));

        return Result.Success(
            message: "{Entity} deleted successfully.",
            statusCode: HttpStatusCode.OK);
    }

    public Result UpdateTitle(string title)
    {
        if (IsDeleted)
            return Result.Failure(
                message: "Cannot update a deleted {entity}.",
                statusCode: HttpStatusCode.BadRequest);

        Title = title;

        Result validationResult = Validate(this);

        if (validationResult.IsFailure)
            return validationResult;

        Raise(new {Entity}TitleUpdatedDomainEvent(Id));

        return Result.Success(
            message: "{Entity} title updated successfully.",
            statusCode: HttpStatusCode.OK);
    }
}
```

### {Entity}.Validate.cs

The validate file is a `partial` class. It has one private entry-point method (`Validate`)
and two private helpers: `ValidateProperties` for field-level rules and `ValidateBusiness`
for cross-field or domain-rule checks.

```csharp
using Core.Library.ResultPattern;
using Core.Library.Validators;
using System.Net;

public partial class {Entity}
{
    private static Result Validate({Entity} {Entity})
    {
        List<Result> results = [];

        ValidateProperties(results, {Entity});
        ValidateBusiness(results, {Entity});

        return Result.CombineValidationErrors(results);
    }

    private static void ValidateProperties(List<Result> results, {Entity} {Entity})
        => results.AddRange(
            [
                {Entity}.Title.Validate(
                    fieldName: nameof({Entity}.Title),
                    maxLength: 200,
                    minLength: 3),

                {Entity}.TenantId.Validate(
                    fieldName:  nameof({Entity}.TenantId),
                    allowEmpty: false)
            ]);

    private static void ValidateBusiness(List<Result> results, {Entity} {Entity})
    {
        if ({Entity}.Title.StartsWith("_"))
            results.Add(Result.Failure(
                message:      "Validation failed",
                errorDetails: ErrorDetails.Create(["Title cannot start with underscore."]),
                statusCode:   HttpStatusCode.UnprocessableEntity));
    }
}
```

### DomainEvents

Each event is a `sealed` class that extends `DomainEvent` from `Core.Library`. It holds
only the entity ID (and any additional identifiers needed by handlers).

```csharp
using Core.Library.Contracts.DomainEvents.Publish;

public sealed class {Entity}CreatedDomainEvent(Guid EntityId) : DomainEvent
{
    public Guid EntityId { get; } = EntityId;
}

public sealed class {Entity}TitleUpdatedDomainEvent(Guid EntityId) : DomainEvent
{
    public Guid EntityId { get; } = EntityId;
}

public sealed class {Entity}DeletedDomainEvent(Guid EntityId) : DomainEvent
{
    public Guid EntityId { get; } = EntityId;
}
```

### Models

Models are `sealed record` types. `CreateModel` carries only the data needed to
instantiate the entity. `UpdateModel` carries identity fields plus nullable update fields.

```csharp
public sealed record {Entity}CreateModel
{
    public required Guid TenantId { get; init; }
    public required string Title { get; init; }
}

public sealed record {Entity}UpdateModel
{
    public required Guid TenantId { get; init; }
    public required Guid EntityId { get; init; }
    public required string? Title { get; init; }
}
```

### Interfaces

`I{Entity}Repository` extends `ITenantedRepository<{Entity}>` (or `IGlobalRepository<{Entity}>` if
the entity has no tenant scope). Entity-specific query methods are declared here.

```csharp
using Core.Library.Contracts.GenericRepositories;
using Core.Library.ResultPattern;

public interface I{Entity}Repository : ITenantedRepository<{Entity}>
{
    Task<Result<{Entity}[]>> GetPaginated{Entities}Async(
        Guid              tenantId,
        int               page,
        int               pageSize,
        string            sortBy,
        bool              isAscending,
        string?           filter,
        CancellationToken cancellationToken = default);
}
```

`I{Entity}Service` and `I{Entity}UseCase` are declared in the same `Interfaces/` folder.
Their method signatures mirror each other — the service performs a single operation,
the use case orchestrates.

---

## Pattern 2 — Reference Data Entity (AutoSeedData)

A reference-data entity represents a fixed lookup table (e.g. statuses, categories) whose
rows are known at design time and must be seeded into the database automatically.

It implements `IHaveAutoSeedData` from `Core.Library` and declares its known instances as
`public static readonly` fields decorated with `[AutoSeedData]`. The infrastructure
persistence layer scans for these at startup and seeds them automatically.

### {Entity}Status.cs

```csharp
using Core.Library.Abstractions;
using Core.Library.Abstractions.Interfaces;
using Core.Library.Attributes;

public class {Entity}Status : BaseEntity, IHaveAutoSeedData
{
    // Constructors
    private {Entity}Status() { }

    // Properties
    public string Name { get; private set; } = default!;

    // Seed Instances
    [AutoSeedData]
    public static readonly {Entity}Status Draft = new()
    {
        Id = Guid.Parse("00000000-0000-0000-0000-000000000001"),
        Name = "Draft"
    };

    [AutoSeedData]
    public static readonly {Entity}Status Confirmed = new()
    {
        Id = Guid.Parse("00000000-0000-0000-0000-000000000002"),
        Name = "Confirmed"
    };

    [AutoSeedData]
    public static readonly {Entity}Status Cancelled = new()
    {
        Id = Guid.Parse("00000000-0000-0000-0000-000000000003"),
        Name = "Cancelled"
    };
}
```

The IDs are hardcoded `Guid` literals so that re-seeding never creates duplicate rows.

---

## Critical Rules

- Every entity must extend `BaseEntity`. No exceptions.
- The parameterless constructor must always be `private`. It exists only for the ORM.
- All factory methods (`Create`) are `public static`. Constructors are never called
  directly from outside the entity class.
- Every `Create` and update method must return `Result` or `Result<T>`. Never throw for
  domain or validation failures.
- `null` model input in a factory method is a programmer error — throw `{Solution}Exception`.
- Validation is always split into `ValidateProperties` (field rules) + `ValidateBusiness`
  (cross-field/domain rules) and merged via `Result.CombineValidationErrors`.
- `IHaveSoftDelete` and `IHaveTenant` are strictly opt-in. Do not add them to an entity
  unless it actually requires soft-delete or tenant scoping.
- `SoftDelete()` must check `IsDeleted` first and return `Result.Failure` if already deleted.
- Domain events are raised **after** all validation passes and state has been mutated.
- Every entity-specific query method goes on the concrete repository interface
  (`I{Entity}Repository`), not on `IBaseRepository` / `IGlobalRepository` / `ITenantedRepository`.
- `IGlobalRepository` and `ITenantedRepository` are mutually exclusive. Choose exactly one.
- Reference data entities must use hardcoded, stable `Guid` literals for their seed IDs.
  Auto-generated IDs break re-seeding idempotency.
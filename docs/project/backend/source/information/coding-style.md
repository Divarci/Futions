# Coding Style

This document establishes the code quality and consistency standards used across the solution. All rules are derived from the actual codebase.

---

## Naming Conventions

### General

| Element | Convention | Example |
|---|---|---|
| Namespace | PascalCase, mirrors folder path | `Core.Domain.Entities.Organisations.{Entities}` |
| Class / Record | PascalCase | `{Entity}Service`, `{Entity}CreateModel` |
| Interface | `I` prefix + PascalCase | `I{Entity}Repository`, `ICacheProvider` |
| Method | PascalCase | `Create{Entity}Async`, `GetPaginated{Entities}Async` |
| Property | PascalCase | `TenantId`, `IsDeleted`, `OccurredOnUtc` |
| Private field | `_camelCase` | `_{entity}Repository`, `_cacheInvalidationService` |
| Parameter | camelCase | `tenantId`, `createModel`, `cancellationToken` |
| Constant | PascalCase | `PolicyNames.AllRoles`, `ApiVersion.V1` |
| Static readonly | PascalCase | `SerializerOptions.Instance` |
| Generic type parameter | `T` prefix | `TDto`, `TEntity`, `TProperty` |

### Async Methods

All async methods are suffixed with `Async`.

```csharp
Task<Result<{Entity}>> Create{Entity}Async(...)
Task<PaginatedResult<TDto[]>> GetPaginated{Entities}Async<TDto>(...)
```

### File Naming

Partial class files use the pattern `ClassName.Operation.cs`:

```
{Entity}Service.cs                   // Constructor + fields
{Entity}Service.Collection.cs        // GetPaginated...
{Entity}Service.Create.cs            // Create...
{Entity}Service.Update.cs            // Update...
{Entity}Service.Delete.cs            // Delete...
{Entity}Service.Single.cs            // GetById...
```

Entity validation goes into `EntityName.Validate.cs`:

```
{Entity}.cs
{Entity}.Validate.cs
```

---

## Vertical Alignment Code Writing

Code is written with a deliberate vertical rhythm. Each logical unit occupies its own line and statements are never compressed onto a single line to save space. The goal is consistent scanability top-to-bottom.

### One Statement Per Line

Each statement, declaration, or chained call appears on its own line.

```csharp
// CORRECT
Result<{Entity}> createResult = {Entity}.Create(createModel);

if (createResult.IsFailureAndNoData)
    return createResult;

await _{entity}Repository.AddAsync(createResult.Data!, cancellationToken);
await _unitOfWork.SaveChangesAsync(cancellationToken);

// INCORRECT — multiple concerns on one line
var result = {Entity}.Create(model); if (result.IsFailureAndNoData) return result;
```

### Constructor Parameters — One Per Line

When a constructor or method call has more than one parameter, each parameter is placed on its own line, indented once from the opening parenthesis.

```csharp
// CORRECT
internal sealed partial class {Entity}Service(
    I{Entity}Repository {entity}Repository,
    ICacheInvalidationService cacheInvalidationService) : I{Entity}Service

// INCORRECT
internal sealed partial class {Entity}Service(I{Entity}Repository {entity}Repository, ICacheInvalidationService cacheInvalidationService) : I{Entity}Service
```

### Field Assignments — One Per Line

Each injected dependency assignment occupies its own line with consistent indentation.

```csharp
// CORRECT
private readonly I{Entity}Repository    _{entity}Repository    = {entity}Repository;
private readonly ICacheInvalidationService _cacheInvalidationService = cacheInvalidationService;

// INCORRECT
private readonly I{Entity}Repository _{entity}Repository = {entity}Repository; private readonly ICacheInvalidationService _cacheInvalidationService = cacheInvalidationService;
```

### Method Chain — One Call Per Line

Fluent chains (LINQ, builder patterns, service registration) break after each method call.

```csharp
// CORRECT
builder.Services
    .RegisterRestApiLayer()
    .RegisterUseCaseLayer()
    .RegisterServiceLayer()
    .RegisterCachingLayer(builder.Configuration)
    .RegisterPersistenceLayer(builder.Configuration);

// INCORRECT
builder.Services.RegisterRestApiLayer().RegisterUseCaseLayer().RegisterServiceLayer();
```

### Conditional Bodies

Single-statement `if` bodies may omit braces but must still sit on their own line, indented below the condition. Multi-statement bodies always use braces.

```csharp
// CORRECT — single statement, own line
if (createResult.IsFailureAndNoData)
    return createResult;

// CORRECT — multi-statement, braces required
if (updateModel.Name is not null)
{
    {entity}.UpdateName(updateModel.Name);
    {entity}.UpdateUpdatedAt(DateTime.UtcNow);
}
```

### Blank Lines Between Logical Blocks

A single blank line separates distinct logical blocks within a method body (guard clauses, core logic, persistence, return).

```csharp
public async Task<Result<{Entity}>> Create{Entity}Async(
    Guid tenantId,
    {Entity}CreateModel createModel,
    CancellationToken cancellationToken)
{
    Result<{Entity}> createResult = {Entity}.Create(createModel);

    if (createResult.IsFailureAndNoData)
        return createResult;

    await _{entity}Repository.AddAsync(createResult.Data!, cancellationToken);
    await _unitOfWork.SaveChangesAsync(cancellationToken);

    return createResult;
}
```

---

## Unused Codes

The codebase must contain **zero unused artefacts**. Dead code is a maintenance liability and obscures intent.

| Category | Rule |
|---|---|
| `using` directives | Remove any `using` that is not referenced in the file. No orphaned namespace imports. |
| Parameters | Every method/constructor parameter must be used inside the method body. Discard unused parameters rather than keeping them for "future use". |
| Variables & locals | Every declared local variable must be read at least once after assignment. |
| Private fields | Every injected or assigned private field must be used in at least one method. |
| Private methods | Every private method must be called from within the class. |
| Classes & interfaces | Every type in the project must be referenced from at least one other type. |
| `.csproj` exclusions | Obsolete source folders are removed from compilation via `<Compile Remove="..." />` rather than left as dead files. |

This rule is enforced at code-review time. IDE warnings for unused symbols must be resolved — not suppressed — before a change is committed.

---

## Class Design

### Access Modifiers

| Type | Modifier |
|---|---|
| Service implementations | `internal sealed partial class` |
| Use case implementations | `internal sealed partial class` |
| Repository implementations | `internal sealed partial class` |
| Domain entity | `public sealed partial class` |
| Value object | `public sealed record` |
| Domain event | `public sealed record` |
| Model (DTO) | `public sealed record` |
| Request / Response (API) | `public record` / `public class` |
| Controller | `public class` |
| Base controller | `public class` |
| Static helper | `internal static class` / `public static class` |
| Domain event handler | `internal sealed class` |
| Exception | `public sealed class` |

### Sealed by Default

All non-abstract implementation classes are `sealed` to prevent unintended inheritance.

### Partial Classes

Entities, services, and use cases use partial classes to split by responsibility:
- The primary partial file contains the constructor, injected fields, and class declaration.
- Each operation gets its own file (Collection, Single, Create, Update, Delete).

### Primary Constructors

Services, use cases, repositories, controllers, and handlers use **primary constructors** with explicit field assignments:

```csharp
internal sealed partial class {Entity}Service(
    I{Entity}Repository {entity}Repository,
    ICacheInvalidationService cacheInvalidationService) : I{Entity}Service
{
    private readonly I{Entity}Repository _{entity}Repository = {entity}Repository;
    private readonly ICacheInvalidationService _cacheInvalidationService = cacheInvalidationService;
}
```

---

## Record Design

Records are the exclusive type for immutable data transfer across layer boundaries and within the domain.

### When to Use a Record

| Use Case | Type |
|---|---|
| Value object | `public sealed record` |
| Domain event | `public sealed record` |
| Application model (DTO) | `public sealed record` |
| Configuration binding | `public sealed record` |
| API request model | `public record` |
| API response model | `public record` |

### Positional vs Property Syntax

**Positional syntax** (primary constructor) is used for simple, fully-required records where all properties are always provided together:

```csharp
public sealed record {Entity}CreateModel(
    Guid TenantId,
    string Name,
    string? Description);
```

**Property syntax** (`{ get; init; }`) is used when individual properties are optional or need data annotations:

```csharp
public record Create{Entity}Request
{
    [Required]
    [MaxLength(100)]
    [JsonPropertyName("name")]
    public string? Name { get; init; }

    [JsonPropertyName("description")]
    public string? Description { get; init; }
}
```

### Immutability

All record properties use `init`-only setters. Records are never mutated after construction — a new instance is produced instead.

### `sealed` on Domain Records

Domain records (`value objects`, `domain events`, `models`) are always `sealed`. API records (`request`, `response`) omit `sealed` to allow test or mapping inheritance where needed.

# Endpoint Pattern

This document shows the complete, copy-ready pattern for a controller, a request, a response, and a mapper. All examples use generic `{Entity}` / `{Module}` placeholders.

---

## Controller — `{Entity}Controller.cs`

**Path:** `Controllers/VersionOne/{Module}/{Entities}/Core/{Entity}Controller.cs`  
*(No `Core/` subfolder when the entity has no nested sub-controllers — see api-structure.md)*

```csharp
using Adapter.RestApi.AspNetCore.Authentication;
using Adapter.RestApi.AspNetCore.Filters;
using Adapter.RestApi.Controllers.Shared.Mappers;
using Adapter.RestApi.Controllers.Shared.Models;
using Adapter.RestApi.Controllers.VersionOne.{Module}.{Entities}.Core.Models.Requests;
using Adapter.RestApi.Controllers.VersionOne.{Module}.{Entities}.Core.Models.Responses;
using Asp.Versioning;
using Core.Domain.Entities.{Module}.{Entities};
using Core.Domain.Entities.{Module}.{Entities}.Interfaces;
using Core.Domain.Entities.{Module}.{Entities}.Models;
using Core.Domain.ValueObjects.AuditStampValueObject;
using Core.Library.ResultPattern;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Adapter.RestApi.Controllers.VersionOne.{Module}.{Entities}.Core;

[ApiVersion(ApiVersion.V1)]
[Route("api/v{version:apiVersion}/tenants/{tenantId:guid}/{entities}")]
[ApiController]
[Authorize(Policy = PolicyNames.AllRoles)]
[TenantAuthorization]
public class {Entity}Controller(
    I{Entity}UseCase {entity}UseCase) : BaseController
{
    private readonly I{Entity}UseCase _{entity}UseCase = {entity}UseCase;

    [HttpGet]
    [ProducesResponseType<PaginatedResult<{Entity}Response[]>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status403Forbidden)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Get{Entities}Async(
        Guid tenantId,
        [FromQuery] PaginationFilterModel filter,
        CancellationToken cancellationToken)
    {
        PaginatedResult<{Entity}Response[]> paginated{Entities} = await _{entity}UseCase.GetPaginated{Entities}Async(
            tenantId,
            filter.Page,
            filter.PageSize,
            filter.SortBy,
            filter.IsAscending,
            filter.Filter,
            {Entity}Mapper.ToArrayResponse,
            cancellationToken);

        return HandleResult(paginated{Entities});
    }

    [HttpGet("{{{entity}Id}}")]
    [ProducesResponseType<Result<{Entity}Response>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status403Forbidden)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Get{Entity}Async(
        Guid tenantId,
        Guid {entity}Id,
        CancellationToken cancellationToken)
    {
        Result<{Entity}Response> {entity} = await _{entity}UseCase.Get{Entity}ByIdAsync(
            tenantId,
            {entity}Id,
            {Entity}Mapper.ToResponse,
            cancellationToken);

        return HandleResult({entity});
    }

    [Authorize(Policy = PolicyNames.AdminOrSystemAdmin)]
    [HttpPost]
    [ProducesResponseType<Result<{Entity}Response>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status403Forbidden)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Create{Entity}Async(
        Guid tenantId,
        [FromBody] Create{Entity}Request request,
        CancellationToken cancellationToken = default)
    {
        {Entity}CreateModel {entity}CreateModel = {Entity}Mapper.ToCreateModel(request, tenantId);
        AuditStampCreateModel auditLogCreateModel = AuditLogMapper.ToCreateModel(
            GetCurrentUserId(),
            GetCurrentUsername(),
            tenantId);

        Result<{Entity}> created{Entity} = await _{entity}UseCase.Create{Entity}Async(
            {entity}CreateModel,
            auditLogCreateModel,
            cancellationToken);

        return HandleResult(
            result: created{Entity},
            mapper: {Entity}Mapper.ToResponse);
    }

    [Authorize(Policy = PolicyNames.AdminOrSystemAdmin)]
    [HttpPatch("{{{entity}Id}}")]
    [ProducesResponseType<Result<{Entity}Response>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status403Forbidden)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Update{Entity}Async(
        Guid tenantId,
        Guid {entity}Id,
        [FromBody] Update{Entity}Request request,
        CancellationToken cancellationToken = default)
    {
        {Entity}UpdateModel {entity}UpdateModel = {Entity}Mapper.ToUpdateModel(request, tenantId, {entity}Id);
        AuditStampCreateModel auditStampCreateModel = AuditLogMapper.ToCreateModel(
            GetCurrentUserId(),
            GetCurrentUsername(),
            tenantId);

        Result updated{Entity} = await _{entity}UseCase.Update{Entity}Async(
            {entity}UpdateModel,
            auditStampCreateModel,
            cancellationToken);

        return HandleResult(result: updated{Entity});
    }

    [Authorize(Policy = PolicyNames.AdminOrSystemAdmin)]
    [HttpDelete("{{{entity}Id}}")]
    [ProducesResponseType<Result>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status403Forbidden)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Delete{Entity}Async(
        Guid tenantId,
        Guid {entity}Id,
        CancellationToken cancellationToken = default)
    {
        AuditStampCreateModel auditStampCreateModel = AuditLogMapper.ToCreateModel(
            GetCurrentUserId(),
            GetCurrentUsername(),
            tenantId);

        Result deleted{Entity} = await _{entity}UseCase.Delete{Entity}Async(
            tenantId,
            {entity}Id,
            auditStampCreateModel,
            cancellationToken);

        return HandleResult(result: deleted{Entity});
    }
}
```

### Controller rules

- Always extend `BaseController`. Never implement `ControllerBase` directly.
- The class-level `[Authorize(Policy = PolicyNames.AllRoles)]` covers all read endpoints. Write endpoints (`POST`, `PATCH`, `DELETE`) declare an additional `[Authorize(Policy = PolicyNames.AdminOrSystemAdmin)]` on the method.
- `[TenantAuthorization]` is applied at the class level on every tenant-scoped controller.
- Route template always follows the pattern `api/v{version:apiVersion}/tenants/{tenantId:guid}/{entities}`.
- All five `HandleResult` overloads live in `BaseController` — never call `Ok()`, `NotFound()`, or `StatusCode()` directly.
- The `mapper` delegate is always a static method reference from `{Entity}Mapper` — never a lambda inline inside the controller.
- `CancellationToken` is always the last parameter and always has `= default` on mutating actions.
- Every action must declare `[ProducesResponseType]` for every status code it can return.

---

## Request — `Create{Entity}Request.cs`

**Path:** `Controllers/VersionOne/{Module}/{Entities}/Core/Models/Requests/Create{Entity}Request.cs`

```csharp
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Adapter.RestApi.Controllers.VersionOne.{Module}.{Entities}.Core.Models.Requests;

public sealed record Create{Entity}Request
{
    [Required, MaxLength(200)]
    [JsonPropertyName("name")]
    public string? Name { get; init; }
}
```

**Path:** `Controllers/VersionOne/{Module}/{Entities}/Core/Models/Requests/Update{Entity}Request.cs`

```csharp
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Adapter.RestApi.Controllers.VersionOne.{Module}.{Entities}.Core.Models.Requests;

public sealed record Update{Entity}Request
{
    [MaxLength(200)]
    [JsonPropertyName("name")]
    public string? Name { get; init; }
}
```

### Request rules

- `Create` requests use `sealed record`. All required fields are nullable with `[Required]` — this lets `ValidationFilter` intercept missing values before controller code runs and produce a clean `422`.
- `Update` requests also use `sealed record`. All fields are nullable without `[Required]` — the absence of a field means "do not change".
- Every property carries `[JsonPropertyName]` with an explicit camelCase name.
- Data annotation constraints (`[MaxLength]`, `[EmailAddress]`, etc.) are placed here, not in the domain layer.
- No business logic, no domain types — request records are pure HTTP surface.

---

## Response — `{Entity}Response.cs`

**Path:** `Controllers/VersionOne/{Module}/{Entities}/Core/Models/Responses/{Entity}Response.cs`

```csharp
using System.Text.Json.Serialization;

namespace Adapter.RestApi.Controllers.VersionOne.{Module}.{Entities}.Core.Models.Responses;

public sealed record {Entity}Response
{
    [JsonPropertyName("id")]
    public required Guid Id { get; init; }

    [JsonPropertyName("name")]
    public required string Name { get; init; }
}
```

### Response rules

- Always `sealed record`. Init-only properties.
- Every property carries `[JsonPropertyName]` with an explicit camelCase name.
- Use `required` on every non-nullable property.
- Nested value object responses use the shared response types from `Controllers/Shared/`.
- Never expose domain entity types or internal model types directly in a response.

---

## Mapper — `{Entity}Mapper.cs`

**Path:** `Controllers/VersionOne/{Module}/{Entities}/Core/{Entity}Mapper.cs`

```csharp
using Adapter.RestApi.Controllers.VersionOne.{Module}.{Entities}.Core.Models.Requests;
using Adapter.RestApi.Controllers.VersionOne.{Module}.{Entities}.Core.Models.Responses;
using Core.Domain.Entities.{Module}.{Entities};
using Core.Domain.Entities.{Module}.{Entities}.Models;

namespace Adapter.RestApi.Controllers.VersionOne.{Module}.{Entities}.Core;

internal static class {Entity}Mapper
{
    internal static {Entity}Response ToResponse({Entity} {entity})
        => new()
        {
            Id   = {entity}.Id,
            Name = {entity}.Name
        };

    internal static {Entity}Response[] ToArrayResponse({Entity}[] {entities})
        => [.. {entities}.Select(ToResponse)];

    internal static {Entity}CreateModel ToCreateModel(Create{Entity}Request request, Guid tenantId)
        => new()
        {
            TenantId = tenantId,
            Name     = request.Name!
        };

    internal static {Entity}UpdateModel ToUpdateModel(Update{Entity}Request request, Guid tenantId, Guid {entity}Id)
        => new()
        {
            TenantId  = tenantId,
            {Entity}Id = {entity}Id,
            Name      = request.Name
        };
}
```

### Mapper rules

- Always `internal static`. Never `public`, never instantiated.
- Four methods are always present: `ToResponse`, `ToArrayResponse`, `ToCreateModel`, `ToUpdateModel`.
- `ToArrayResponse` uses collection expression syntax (`[.. source.Select(ToResponse)]`).
- The `!` null-forgiving operator is used on `[Required]` request fields because `ValidationFilter` guarantees they cannot be null by the time the mapper runs.
- Nullable update fields are passed as-is — the domain layer decides what to do with a `null` value.
- Shared value object mapping always delegates to the corresponding mapper in `Controllers/Shared/` (e.g., `AddressMaper.ToResponse(...)`).

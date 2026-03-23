# AspNetCore — Infrastructure Wiring for the REST API

**Path:** `Api/Source/Adapter/Adapter.RestApi/AspNetCore/`

This folder contains the cross-cutting framework plumbing for the REST API adapter: authentication configuration, authorization filters, HTTP context utilities, global error handling, and result-to-HTTP translation. None of these files contain business logic — they all exist to wire the web framework to the rest of the architecture.

```
AspNetCore/
├── Authentication/
│   ├── JwtBearerConfigurationOptions.cs
│   ├── PolicyNames.cs
│   └── Role.cs
├── Diagnostics/
│   └── GlobalExceptionHandler.cs
├── Extensions/
│   └── HttpContextExtensions.cs
├── Filters/
│   ├── TenantAuthorizationAttribute.cs
│   └── ValidationFilter.cs
└── Helpers/
    └── ApiResultHelper.cs
```

---

## Authentication

**Path:** `Api/Source/Adapter/Adapter.RestApi/AspNetCore/Authentication/`

Holds the three types that define the authentication and authorization surface of the API.

### `JwtBearerConfigurationOptions.cs`

Implements `IConfigureNamedOptions<JwtBearerOptions>`. Reads the `Authentication` section from `appsettings` and binds it directly onto `JwtBearerOptions` at startup. This removes the need for inline `AddJwtBearer` lambda configuration in `ServiceRegistrar` and makes all token validation settings configurable via environment variables.

### `PolicyNames.cs`

Static class with three `const string` fields: `AllRoles`, `AdminOrSystemAdmin`, and `SystemAdmin`. These are the named authorization policy identifiers referenced on controller actions. Centralising them here prevents magic strings from spreading across controllers.

### `Role.cs`

Static class with three `const string` fields: `UserRole`, `AdminRole`, `SystemAdminRole`. These are the JWT role claim values matched by the authorization policies. Used by `TenantAuthorizationAttribute` and anywhere role-based checks are needed.

---

## Diagnostics

**Path:** `Api/Source/Adapter/Adapter.RestApi/AspNetCore/Diagnostics/`

Contains the last-resort error boundary for the HTTP pipeline.

### `GlobalExceptionHandler.cs`

Implements `IExceptionHandler`. Catches every unhandled exception that escapes the controller pipeline. Generates a `traceId` (`Guid.NewGuid()`), writes a `500 Internal Server Error` response as a `ProblemDetails` JSON body, and logs the exception. Distinguishes `{Solution}Exception` (logs assembly, class, method, and message as structured fields) from all other exceptions (logs with full stack trace). The `traceId` appears in both the response body and the log entry for correlation.

---

## Extensions

**Path:** `Api/Source/Adapter/Adapter.RestApi/AspNetCore/Extensions/`

Utility methods for extracting authenticated identity from the request context.

### `HttpContextExtensions.cs`

Static class with six extension methods on `HttpContext`, organised in pairs (nullable + required) for three claim types:

| Method | Returns | Behaviour on missing claim |
|---|---|---|
| `GetTenantId()` | `Guid?` | Returns `null` |
| `GetRequiredTenantId()` | `Guid` | Throws `{Solution}Exception` |
| `GetUserId()` | `Guid?` | Returns `null` |
| `GetRequiredUserId()` | `Guid` | Throws `{Solution}Exception` |
| `GetUserEmail()` | `string?` | Returns `null` |
| `GetRequiredUserEmail()` | `string` | Throws `{Solution}Exception` |

A seventh method, `IsInRole(string roleName)`, delegates to `ClaimsPrincipal.IsInRole`. Controllers use the nullable variants when a claim is optional; filter and middleware code that runs behind `[Authorize]` uses the `Required` variants.

---

## Filters

**Path:** `Api/Source/Adapter/Adapter.RestApi/AspNetCore/Filters/`

Action filters that enforce security and input-validation rules before controller code runs.

### `TenantAuthorizationAttribute.cs`

`ActionFilterAttribute` applied at the class or method level. Runs on every request to a tenant-scoped route. Compares the `tenantId` route parameter against the `tenantId` claim in the JWT token:

- If the caller has the `SystemAdmin` role, the check is bypassed entirely.
- If the route `tenantId` cannot be parsed as a `Guid`, returns `400 Bad Request`.
- If the JWT carries no `tenantId` claim, returns `401 Unauthorized`.
- If the two IDs do not match, returns `404 Not Found` (intentional — avoids disclosing that a different tenant's resource exists).

### `ValidationFilter.cs`

`IActionFilter` registered globally. Runs `OnActionExecuting` before every controller action. If `ModelState` is invalid (i.e. model binding or `[Required]` / data annotation validation failed), it short-circuits the pipeline and returns a `422 Unprocessable Entity` `ProblemDetails` response with all field errors grouped by field name. No action method code executes when this filter fires.

---

## Helpers

**Path:** `Api/Source/Adapter/Adapter.RestApi/AspNetCore/Helpers/`

Translation utilities between the internal Result Pattern and HTTP responses.

### `ApiResultHelper.cs`

Static class with a single method, `Problem(Result result)`. Converts a failed `Result` into a `ProblemDetails` object following RFC 7807. Maps the `Result.StatusCode` to the appropriate RFC `type` URI, sets `title` from `Result.Message`, and attaches `ErrorDetails.Errors` under the `errors` extension key when present. Throwing on a successful result (checked at the top of the method) ensures this helper is never accidentally called on a passing operation.

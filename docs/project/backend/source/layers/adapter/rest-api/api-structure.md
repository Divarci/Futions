# Adapter.RestApi — Structure

The REST API adapter is the outermost layer of the backend. It owns all HTTP concerns: routing, request/response models, authentication configuration, authorization filters, and HTTP pipeline wiring. It depends on the Application layer through use case interfaces and never references Infrastructure directly.

---

## Tree

```
Adapter.RestApi/
├── Program.cs
├── ServiceRegistrar.cs
├── appsettings.json
├── appsettings.Development.json
├── Dockerfile
│
├── AspNetCore/                          ← framework plumbing (see asp-net-core.md)
│   ├── Authentication/
│   │   ├── JwtBearerConfigurationOptions.cs
│   │   ├── PolicyNames.cs
│   │   └── Role.cs
│   ├── Diagnostics/
│   │   └── GlobalExceptionHandler.cs
│   ├── Extensions/
│   │   └── HttpContextExtensions.cs
│   ├── Filters/
│   │   ├── TenantAuthorizationAttribute.cs
│   │   └── ValidationFilter.cs
│   └── Helpers/
│       └── ApiResultHelper.cs
│
└── Controllers/
    ├── ApiVersion.cs
    ├── BaseController.cs
    ├── Shared/                          ← shared value object mappers, requests, responses
    │   ├── {ValueObject}/
    │   │   ├── {ValueObject}Mapper.cs
    │   │   ├── Requests/
    │   │   │   └── Update{ValueObject}Request.cs
    │   │   └── Responses/
    │   │       └── {ValueObject}Response.cs
    │   ├── Mappers/
    │   │   └── AuditLogMapper.cs
    │   └── Models/
    │       └── PaginationFilterModel.cs
    └── VersionOne/                      ← v1 controllers, one folder per domain module
        └── {Module}/
            │
            ├── {Entities}/              ← entity WITHOUT nested sub-entity controllers
            │   ├── {Entity}Controller.cs
            │   ├── {Entity}Mapper.cs
            │   └── Models/
            │       ├── Requests/
            │       │   ├── Create{Entity}Request.cs
            │       │   └── Update{Entity}Request.cs
            │       └── Responses/
            │           └── {Entity}Response.cs
            │
            └── {Entities}/              ← entity WITH nested sub-entity controllers
                ├── Core/                ← main entity controller lives here
                │   ├── {Entity}Controller.cs
                │   ├── {Entity}Mapper.cs
                │   └── Models/
                │       ├── Requests/
                │       │   ├── Create{Entity}Request.cs
                │       │   └── Update{Entity}Request.cs
                │       └── Responses/
                │           └── {Entity}Response.cs
                └── {NestedEntities}/    ← one folder per nested sub-entity
                    ├── {NestedEntity}Controller.cs
                    ├── {NestedEntity}Mapper.cs
                    └── Models/
                        ├── Requests/
                        │   ├── Create{NestedEntity}Request.cs
                        │   └── Update{NestedEntity}Request.cs
                        └── Responses/
                            └── {NestedEntity}Response.cs
```

---

## Critical Rules

- **No business logic.** Controllers, mappers, and request/response models must not contain domain decisions. All domain work is delegated to the use case layer.
- **One controller per entity boundary.** Each entity or aggregate root gets its own controller class. Do not merge unrelated endpoints into a single controller.
- **Versioning via folder, not attribute chaos.** All v1 endpoints live under `Controllers/VersionOne/`. When a new API version is needed, add a `VersionTwo/` sibling — never mix version logic inside an existing controller.
- **Module mirroring.** The folder layout under `Controllers/VersionOne/` mirrors the module structure in `Core.Domain/Entities/`. Every new domain module gets a matching folder here.
- **Shared value objects belong in `Controllers/Shared/`.** Any request, response, or mapper type used by more than one controller lives there. No duplication across entity folders.
- **`AspNetCore/` is framework plumbing only.** Nothing in that folder knows about domain entities or use cases. It wires the web framework; it does not orchestrate business operations.
- **`bin/` and `obj/` are build artifacts.** They are never part of the structure and must not be committed.
- **`ServiceRegistrar.cs` is the single DI registration point** for this adapter. All controller-layer registrations (filters, versioning, authentication, authorization policies) are defined there.
- **`Program.cs` calls `ServiceRegistrar` only.** No inline service registration in `Program.cs`.

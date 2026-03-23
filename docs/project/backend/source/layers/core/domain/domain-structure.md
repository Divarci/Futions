# Core.Domain — Structure

Holds all domain entities and value objects. Depends only on `Core.Library`.
Every module lives under `Entities/[Module]/`, every value object under `ValueObjects/`.

---

## Tree

```
Core.Domain/
├── Entities/
│   ├── {Module}/                        ← one folder per business module
│   │   ├── {Entity}/
│   │   │   ├── {Entity}.cs
│   │   │   ├── {Entity}.Validate.cs
│   │   │   ├── DomainEvents/
│   │   │   │   ├── {Entity}CreatedDomainEvent.cs
│   │   │   │   ├── {Entity}{Action}DomainEvent.cs
│   │   │   │   └── {Entity}DeletedDomainEvent.cs
│   │   │   ├── Interfaces/
│   │   │   │   ├── I{Entity}Repository.cs
│   │   │   │   ├── I{Entity}Service.cs
│   │   │   │   └── I{Entity}UseCase.cs
│   │   │   └── Models/
│   │   │       ├── {Entity}CreateModel.cs
│   │   │       └── {Entity}UpdateModel.cs
│   │   └── ...                          ← new entity folders go here
│   ├── System/                          ← infrastructure-supporting entities (closed)
│   │   ├── AuditLogs/
│   │   │   ├── AuditLog.cs
│   │   │   ├── AuditLog.Validate.cs
│   │   │   └── Interfaces/
│   │   │       ├── IAuditLogRepository.cs
│   │   │       ├── IAuditLogService.cs
│   │   │       └── IAuditLogUseCase.cs
│   │   └── OutboxMessages/
│   │       ├── OutboxMessage.cs
│   │       ├── OutboxMessage.Validate.cs
│   │       └── Interfaces/
│   │           ├── IOutboxMessageRepository.cs
│   │           ├── IOutboxMessageService.cs
│   │           └── IOutboxProcessor.cs
│   └── ...                              ← new business modules go here (sibling of {Module})
└── ValueObjects/
    ├── {ValueObject}ValueObject/
    │   ├── {ValueObject}.cs
    │   ├── {ValueObject}.Validation.cs
    │   └── {ValueObject}Model.cs
    └── ...                              ← new value objects go here
```

---

## Critical Rules

- Every new business module gets its own folder directly under `Entities/`. No nesting
  modules inside other business modules.
- Every entity folder must follow the exact layout: root files, `DomainEvents/`,
  `Interfaces/`, `Models/`. No ad-hoc sub-folders.
- `System/` is reserved for infrastructure-supporting entities (`AuditLog`, `OutboxMessage`).
  Business entities must never be placed there.
- Every entity interface (`IRepository`, `IService`, `IUseCase`) is declared in the
  entity's own `Interfaces/` folder — not in a shared location.
- Every value object gets its own `{Name}ValueObject/` folder directly under `ValueObjects/`.
  Value objects must never be nested inside entity folders.
- `bin/` and `obj/` directories are build artifacts. They are never part of the structure.
- `Core.Domain` references only `Core.Library`. No references to Application,
  Infrastructure, or Adapter layers are permitted.

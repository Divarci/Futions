# Core.Library — Structure

The shared foundation of the entire backend. Has zero project references and no
infrastructure dependencies. Every other layer depends on it.

---

## Tree

```
Core.Library/
├── Abstractions/
│   ├── BaseEntity.cs
│   └── Interfaces/
│       ├── IHaveSoftDelete.cs
│       ├── IHaveTenant.cs
│       ├── IHaveAutoSeedData.cs
│       ├── IOutboxOptions.cs
│       └── ...                          ← new cross-cutting marker interfaces go here
├── Attributes/
│   ├── AutoSeedDataAttribute.cs
│   └── ...                              ← new infrastructure-driving attributes go here
├── Contracts/
│   ├── Caching/
│   │   ├── ICacheProvider.cs
│   │   └── ICacheInvalidationService.cs
│   ├── DomainEvents/
│   │   ├── Publish/
│   │   │   ├── IDomainEvent.cs
│   │   │   └── DomainEvent.cs
│   │   └── Handle/
│   │       ├── IDomainEventHandler.cs
│   │       └── DomainEventHandler.cs
│   ├── GenericRepositories/
│   │   ├── IBaseRepository.cs
│   │   ├── IGlobalRepository.cs
│   │   └── ITenantedRepository.cs
│   ├── UnitOfWorks/
│   │   └── ITransactionalUnitOfWork.cs
│   └── ...                              ← new capability contracts go in new sub-folders here
├── Exceptions/
│   └── {Solution}Exception.cs
├── ResultPattern/
│   ├── Result.cs
│   ├── ErrorDetails.cs
│   ├── Metadata.cs
│   └── ResultExtensions.cs
└── Validators/
    ├── StringValidator.cs
    ├── GuidValidator.cs
    ├── DateTimeValidator.cs
    ├── DecimalValidator.cs
    ├── IntegerValidator.cs
    ├── EnumValidator.cs
    ├── ValueObjectValidator.cs
    ├── Enums/
    │   ├── DateTimeRestriction.cs
    │   └── ...                          ← new validator-supporting enums go here
    └── ...                              ← new primitive-type validators go here
```

---

## Critical Rules

- Core.Library must have zero project references at all times. Adding a reference to any
  other layer is not permitted.
- New cross-cutting concerns implemented via entity opt-in must go into Abstractions/Interfaces/
  as marker or behavioral interfaces, not directly into BaseEntity.
- New infrastructure capabilities (file storage, messaging, search, etc.) each get their
  own sub-folder under Contracts/. Do not add their interfaces into an existing folder.
- New validator types follow the pattern of existing validators: one static class per
  primitive type, placed directly under Validators/. Supporting enums go into
  Validators/Enums/.
- ResultPattern/ is closed to structural changes. Do not add new files there without a
  deliberate architectural decision.
- Exceptions/ holds one type only. New exception types are not added; `{Solution}Exception`
  is the single application-level exception for programmer errors.

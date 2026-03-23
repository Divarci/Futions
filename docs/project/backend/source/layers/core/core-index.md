# Core Layer — Index

The Core layer is the foundation of the entire backend. It has no outward dependencies —
everything else depends on it. It is split into two projects: `Core.Library` (shared
primitives with zero project references) and `Core.Domain` (domain entities and value
objects, depends only on `Core.Library`).

Use this index to navigate directly to the topic you need.

---

## Core.Library

Shared building blocks used by every layer. Contains zero project references.

| Document | What it covers |
|---|---|
| [Structure](library/library-structure.md) | Full folder tree of `Core.Library` with scale points and critical rules |
| [Abstractions](library/abstractions.md) | `BaseEntity`, `IHaveSoftDelete`, `IHaveTenant`, `IHaveAutoSeedData`, `IOutboxOptions` |
| [Attributes](library/attributes.md) | `AutoSeedDataAttribute` — marks enum fields for automatic DB seeding |
| [Contracts](library/contracts.md) | All technology-agnostic interfaces: caching, domain events, repositories, unit of work |
| [Result Pattern](library/result-pattern.md) | `Result`, `Result<T>`, `PaginatedResult<T>`, `ErrorDetails`, `Metadata`, `ResultExtensions` |
| [Validators](library/validators.md) | Static extension validators for all primitive types and value objects |
| [Exceptions](library/exceptions.md) | `{Solution}Exception` — single application-level exception for programmer errors |

---

## Core.Domain

All domain entities and value objects. Depends only on `Core.Library`.

| Document | What it covers |
|---|---|
| [Structure](domain/domain-structure.md) | Full folder tree of `Core.Domain` with module layout, scale points, and critical rules |
| [Entity Pattern](domain/domain-patterns-entity.md) | Complete A-to-Z structure of a business entity and a reference-data (seed) entity |
| [Value Object Pattern](domain/domain-patterns-value-object.md) | Complete A-to-Z structure of a value object (`sealed partial record`, factory, validation) |

---

## Guidelines

- Read the **Structure** document for either project before adding, moving, or renaming files.
- Read the **Entity Pattern** or **Value Object Pattern** before implementing any new domain type.
- The **Contracts** document is the first stop when wiring up any new infrastructure capability.
- The **Result Pattern** document governs every method return type across all layers — read it once and keep it as a reference.

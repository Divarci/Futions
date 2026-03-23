# Infrastructure Layer — Index

The Infrastructure layer provides all technology-specific implementations: database persistence and distributed caching. It depends on the Core layer and is never referenced by the Application or Adapter layers directly — all coupling goes through the interfaces defined in `Core.Library.Contracts`.

Use this index to navigate directly to the topic you need.

---

## Infra.Persistence

EF Core persistence: repositories, unit of work, query extensions, and entity configuration.

| Document | What it covers |
|---|---|
| [Structure](persistence/persistence-structure.md) | Full folder tree of `Infra.Persistence` with key folder descriptions |
| [Configurations](persistence/configurations.md) | `IEntityTypeConfiguration<T>` files, how they are auto-applied by `AppDbContext`, and conventions |
| [Generic Repository Pattern](persistence/repository-patterns-generic.md) | `BaseRepository`, `TenantedRepository`, `GlobalRepository` — hierarchy, responsibilities, and scale points |
| [Entity Repository Pattern](persistence/repository-patterns-entity.md) | How concrete repositories are structured using partial files |
| [Unit of Work Pattern](persistence/unit-of-work-pattern.md) | Transaction lifecycle, `SaveChangesAsync` ownership, outbox integration, and usage in UseCases |
| [IQueryable Extensions](persistence/extensions.md) | `WhereIf`, `OrderByIf`, `IncludeIf` — conditional query composition and how to extend |

---

## Infra.Caching

Distributed cache: configuration, service implementation, and invalidation strategy.

| Document | What it covers |
|---|---|
| [Caching Overview](caching/caching-info.md) | Strategy, read/write paths, key structure, error resilience, and layer responsibilities |

---

## Guidelines

- Read the **Structure** document for either project before adding or moving files.
- Read the **Generic Repository Pattern** and **Entity Repository Pattern** before implementing any new repository.
- The **Unit of Work Pattern** document governs all write operations — no code should call `SaveChangesAsync` outside it.
- The **IQueryable Extensions** document is the only place to add new conditional query helpers.
- The **Caching Overview** document defines all caching decisions — strategy, key structure, invalidation rules, and layer responsibilities.

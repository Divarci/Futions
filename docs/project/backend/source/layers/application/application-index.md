# Application Layer — Index

The Application layer contains all business orchestration logic: services that operate on
single entities, use cases that coordinate multiple services within a transaction, and
background processors that handle deferred work. It depends on the Core layer for domain
types and contracts, and never references Infrastructure or Adapter code directly.

Use this index to navigate directly to the topic you need.

---

## App.Services

Single-entity operation logic: validation, persistence delegation, and cache invalidation.

| Document | What it covers |
|---|---|
| [Structure](services/service-structure.md) | Folder tree, partial file convention, and DI registration |
| [Base Pattern](services/service-patterns-base.md) | Constructor anatomy, responsibilities, and critical rules |
| [Create Pattern](services/service-patterns-create.md) | Domain factory call, persist, invalidate cache |
| [Update Pattern](services/service-patterns-update.md) | Conditional field updates, cache invalidation |
| [Delete Pattern](services/service-patterns-delete.md) | Pre-deletion checks, repository delete, cache invalidation |
| [Single Pattern](services/service-patterns-single.md) | Repository fetch by ID, `Result<TDto>` return |
| [Collection Pattern](services/service-patterns-collection.md) | Paginated fetch with count, `PaginatedResult<TDto[]>` return |

---

## App.UseCases

Transaction orchestration, cache read-through, and audit logging across entity operations.

| Document | What it covers |
|---|---|
| [Structure](use-cases/use-case-structure.md) | Folder tree, partial file convention, `Helpers/`, `Scheduling/`, and DI registration |
| [Base Pattern](use-cases/use-cases/use-case-patterns-base.md) | Constructor anatomy, five standard dependencies, `_timeout`, and critical rules |
| [Create Pattern](use-cases/use-cases/use-case-patterns-create.md) | `ExecuteTransactionAsync<{Entity}>`, service create, audit log |
| [Update Pattern](use-cases/use-cases/use-case-patterns-update.md) | Non-generic transaction, service update, audit log |
| [Delete Pattern](use-cases/use-cases/use-case-patterns-delete.md) | Tenant-scoped delete, audit log |
| [Single Pattern](use-cases/use-cases/use-case-patterns-single.md) | Cache read-through via `ICacheProvider.GetSingleAsync` |
| [Collection Pattern](use-cases/use-cases/use-case-patterns-collection.md) | Pagination normalisation, cache read-through via `GetPaginatedCollection` |

---

## Helpers

| Document | What it covers |
|---|---|
| [CacheKeyHelper](use-cases/helpers/cache-key-helper.md) | `Single` and `Collection` key builders — format, delegate usage, and scale rules |

---

## Scheduling

Background work processors and domain event handlers.

| Document | What it covers |
|---|---|
| [Overview](use-cases/scheduling/schedule.md) | Role of the `Scheduling/` folder, processor + handler pattern, how to add new background work |
| [Domain Event Handlers](use-cases/scheduling/domain-events/use-case-patterns-handler.md) | Handler anatomy, service-only rule, DI auto-registration |
| [Outbox Processor](use-cases/scheduling/domain-events/outbox-processor.md) | How domain events reach the outbox, processing flow, handler resolution and caching |

---

## Guidelines

- Read the **Structure** document for either project before adding or moving files.
- Read the **Base Pattern** before implementing any new service or use case.
- Every write operation must be wrapped in `ExecuteTransactionAsync` — the **Create, Update, Delete** pattern documents govern this without exception.
- The **CacheKeyHelper** document is the single source of truth for cache key format — never build key strings inline.
- The **Outbox Processor** document governs all domain event dispatch — no handler should be wired outside this mechanism.

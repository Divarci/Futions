# Contracts

## What It Does

Defines all technology-agnostic interfaces for infrastructure capabilities: caching,
domain event publishing and handling, generic repository access, and unit of work
coordination. No implementations exist here — only contracts. This decouples the domain
and application layers from any specific infrastructure technology.

---

## Current Structure

### Caching

Two interfaces govern cache access:

- ICacheProvider — read-through cache. Provides GetSingleAsync<TEntity> for fetching a
  single cached entity, and GetPaginatedCollection<TDto> for paginated list results. Both
  accept a useCache flag, a cache key, a fallback service method delegate, and an expiration
  value. The fallback is called on a cache miss.
- ICacheInvalidationService — cache invalidation. InvalidateEntity(cacheKey) removes a
  single cached item. InvalidateCollections() removes all keys matching the collection_*
  naming pattern.

### DomainEvents / Publish

Two types define the event publication contract:

- IDomainEvent — the marker interface for all domain events. Carries Guid Id and
  DateTime OccurredOnUtc.
- DomainEvent — abstract base class implementing IDomainEvent. The default constructor
  auto-generates a new Guid and captures DateTime.UtcNow. A parameterized constructor
  accepts existing values to support rehydration from storage during outbox replay.

### DomainEvents / Handle

Two interfaces and a base class govern event handling:

- IDomainEventHandler<TDomainEvent> — generic handler contract with a typed
  Handle(TDomainEvent, CancellationToken) method.
- IDomainEventHandler — non-generic variant used by the dispatcher to invoke handlers
  without knowing the concrete event type at compile time.
- DomainEventHandler<TDomainEvent> — abstract base class implementing both interfaces.
  The non-generic Handle casts the incoming event to TDomainEvent and delegates to the
  typed override that concrete handlers implement.

### GenericRepositories

Three interfaces form the repository contract hierarchy:

- IBaseRepository<T> — write-only base. Provides CreateAsync, CreateRangeAsync,
  Update, UpdateRange, Delete, DeleteRange.
- IGlobalRepository<T> — extends IBaseRepository<T> for entities without tenant scope.
  Adds GetByIdAsync(id), ExistsAsync(id), CountAsync().
- ITenantedRepository<T> — extends IBaseRepository<T> for tenant-scoped entities.
  Adds GetByIdAsync(id, tenantId), ExistsAsync(id, tenantId), CountAsync(tenantId).

### UnitOfWorks

- ITransactionalUnitOfWork — wraps a delegate inside a database transaction. Commits on
  success and rolls back on failure. Provides two overloads: one that returns Result for
  command-only operations, and one that returns Result<T> for operations that also produce
  a value.

---

## How to Scale

For each new infrastructure capability (e.g., file storage, messaging, search), add a new
sub-folder under Contracts with a dedicated interface. Keep each folder focused on a
single concern. Do not combine multiple capabilities into one interface.

For new repository query patterns needed across multiple entities, extend the appropriate
base interface here. For entity-specific queries (e.g., GetByEmailAsync), define those
on a concrete repository interface in Core.Domain, not here.

---

## Critical Rules

- No implementations belong here — interfaces and abstract domain base types only.
- Core.Library has zero project references. Never introduce one.
- IGlobalRepository and ITenantedRepository are mutually exclusive. Each entity's
  repository must implement exactly one.
- DomainEvent (abstract class) is the only non-interface type permitted in Contracts.
  It is a pure domain type with no infrastructure dependency.
- Entity-specific query methods do not belong here. They belong on concrete repository
  interfaces defined in Core.Domain.

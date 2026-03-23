# Caching — How It Works

**Files:** All files are located in `Api\Source\Infrastructure\Infra.Caching\`

## Overview

The caching layer follows a **cache-aside** strategy. All cache read operations happen at the Use Case layer, while all cache invalidation operations happen at the Service layer. There is no AOP decorator or pipeline — the interactions are explicit and deliberate.

---

## Conceptual Model

The system separates two concerns into two distinct contracts:

- **`ICacheProvider`** — the read side. Responsible for retrieving data, either from the cache or, on a miss, from the underlying data source.
- **`ICacheInvalidationService`** — the write side. Responsible for removing stale entries after any mutating operation.

Both contracts are satisfied by a single implementation class, `CacheService`, registered as a singleton under both interfaces. This ensures there is always one shared connection in the application.

---

## Read Path — Cache-Aside

When a use case needs data, it does not call the service directly. Instead it goes through `ICacheProvider`, which applies the following logic:

1. **Look up** the cache key.
2. If found (**cache hit**), deserialize the stored value and return it immediately without touching the database.
3. If not found (**cache miss**), delegate to the service method, store the result with an expiration, and then return the result.

This pattern guarantees that the database is only consulted when the cache does not have a fresh answer.

**Expiration times** are set per entity type by the use case that owns the read operation:
- Master data (e.g., companies, persons) — **1 hour**
- Relationship data (e.g., company–person links) — **30 minutes**

---

## Write Path — Invalidation After Mutation

After any create, update, or delete operation, the service layer always performs two invalidation steps:

1. **Invalidate the specific entity key** — the exact key for the affected record is deleted, ensuring no stale single-entity responses survive.
2. **Invalidate all collection keys** — every key whose name starts with `collection_` is scanned and removed in bulk.

The reason collection invalidation is global (not scoped to a single entity type) is that entity collections can carry cross-domain data. For example, a paginated list of companies might embed person counts, or a list of persons might include their associated companies. Wiping all collection keys prevents any cross-entity inconsistency after a mutation, at the cost of slightly more cache misses on the next reads.

---

## Cache Key Structure

Cache keys follow two formats, both fully lowercased:

**Single-entity key**  
Identifies one specific record. It encodes the entity type and all the identifiers needed to uniquely locate it (e.g., tenant ID + entity ID).  
Example shape: `{entity}:{label}({value}):...`

**Collection key**  
Identifies a paginated or filtered list. It always starts with the `collection_` prefix, followed by the entity type, the method name, and the query parameters.  
Example shape: `collection_{entity}:{methodname}_{param}_...`

The `collection_` prefix is what makes bulk invalidation possible — the cache provider can scan for that pattern without needing to track which keys were written.

---

## Error Resilience

All cache interactions are wrapped in error handling. If an error occurs during any read, write, or scan operation, the system logs a warning and **degrades gracefully**:

- On a read failure, the request falls through to the database as if there were a cache miss.
- On a write or invalidation failure, the operation is silently skipped — the mutation itself is not rolled back.

This design ensures that a cache provider outage never causes the application to fail; it only causes a temporary increase in database load.

---

## Infrastructure Registration

The caching layer is self-registering. Its `ServiceRegistrar` binds connection settings from the `CacheConfig` configuration section, creates a single provider singleton, and maps both cache interfaces to the same `CacheService` singleton instance.

---

## Summary of Responsibilities by Layer

| Layer | Role |
|---|---|
| **Core.Library** | Defines `ICacheProvider` and `ICacheInvalidationService` contracts |
| **App.UseCases** | Owns the read path — calls `ICacheProvider`, builds cache keys via `CacheKeyHelper` |
| **App.Services** | Owns the invalidation path — calls `ICacheInvalidationService` after every mutation |
| **Infra.Caching** | Implements both contracts; handles serialization and error resilience |

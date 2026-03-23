# CacheKeyHelper

## Role

`CacheKeyHelper` is an `internal static` class that centralises cache key construction
for the entire `App.UseCases` assembly. Every UseCase that reads from or invalidates the
cache calls one of its two methods to obtain a key — no UseCase ever builds a raw cache
key string inline.

Keeping key format logic in one place means:
- Keys are always lowercase and deterministic.
- The `collection_` prefix used by `ICacheInvalidationService.InvalidateCollections()` is
  applied consistently without each caller remembering to add it.
- If the key format ever changes, only this file needs to be updated.

**Source:** `Api/Source/Application/App.UseCases/Helpers/CacheKeyHelper.cs`

---

## Methods

### `Single`

Builds a key that uniquely identifies **one entity record**.

```
{entityName}:{label1}({value1}):{label2}({value2})...
```

All output is lowercased. Each caller supplies the entity name and one or more
label-value pairs that together uniquely identify the record (typically `tenant` + the
entity's own ID).

**Used in:** every UseCase single-read method and passed as a delegate to service write
methods for post-write invalidation.

Example output:
```
{entity}:tenant({tenantId}):{entity}({entityId})
```

---

### `Collection`

Builds a key that identifies **one specific paginated query result**.

```
collection_{entityName}:{methodName}_{param1}_{param2}_...
```

The `collection_` prefix is mandatory — it is the discriminator that allows
`ICacheInvalidationService.InvalidateCollections()` to purge all collection caches for an
entity with a single wildcard sweep, without touching single-entity keys.

**Used in:** every UseCase paginated-collection method.

Example output:
```
collection_{entity}:getpaginated{entities}async_{tenantId}_{page}_{size}_{sortBy}_{ascending}_{filter}
```

---

## Usage in UseCases

Both methods are passed as **delegates** to service calls, not called to produce a key
that the UseCase then holds. This keeps cache key construction as close as possible to
where the key is consumed.

```csharp
// Single — passed as delegate to the service (write path)
Result<{Entity}> result = await _{entity}Service
    .Create{Entity}Async(createModel, CacheKeyHelper.Single, cancellationToken);

// Single — called directly in the UseCase (read path)
string cacheKey = CacheKeyHelper.Single(
    nameof({Entity}),
    ("tenant", tenantId),
    ("{entity}", {entity}Id));

// Collection — called directly in the UseCase (read path)
string cacheKey = CacheKeyHelper.Collection(
    nameof({Entity}),
    nameof(GetPaginated{Entities}Async),
    [tenantId, page, size, sortBy, ascending, filterQuery ?? string.Empty]);
```

---

## How It Scales

`CacheKeyHelper` scales by staying completely passive — it has no state, no dependencies,
and no knowledge of which entities exist. Adding a new entity or a new UseCase requires
zero changes to this class.

**New entities** — Any new `{Entity}UseCase` calls `CacheKeyHelper.Single` and
`CacheKeyHelper.Collection` with its own entity name and identifier segments. The key
format is identical; only the values differ. No registration or modification of
`CacheKeyHelper` is needed.

**New key segments** — `Single` accepts a variadic array of label-value pairs. If a new
entity's identity requires more segments than the standard `tenant + entityId` pair (e.g.
an entity with a composite key), the caller simply adds more pairs. The method signature
does not change.

**New collection query parameters** — `Collection` accepts a variadic `params object[]`
array. Adding a new filter or sort dimension means passing one more element to the array.
The method signature does not change.

**Key uniqueness** — Because both methods include all discriminating parameters in the
key, two different queries for the same entity will always produce different keys. There
is no risk of cache collision regardless of how many entities or query combinations are
added.

---

## Critical Rules

- Never construct cache key strings inline inside a UseCase or service. Always call
  `CacheKeyHelper.Single` or `CacheKeyHelper.Collection`.
- The `collection_` prefix must only appear through `CacheKeyHelper.Collection` — never
  hardcode it manually.
- Do not add new key formats to this helper unless they represent a genuinely distinct
  access pattern. Single and collection cover all current read patterns.

# UseCase Patterns — Collection

## Logic

1. **Normalise pagination inputs.** Raw query parameters arrive as nullable primitives
   from the adapter layer. The UseCase applies defaults before any service or cache call:
   page defaults to `1` when `null` or less than `1`; page size defaults to `25`; sort
   column defaults to `"Id"`; sort direction defaults to ascending.
2. **Build a cache key** via `CacheKeyHelper.Collection`, supplying the entity name, the
   method name, and all normalised parameters as a flat array. The resulting key is
   unique per combination of tenant, page, size, sort, and filter — so different query
   permutations are cached independently.
3. Call `_cacheProvider.GetPaginatedCollection`, passing the constructed key, `useCache:
   true`, the service fetch as a delegate, and `_timeout`.
4. The cache provider checks the cache first. On a miss it invokes the service, stores the
   result, and returns it. On a hit it returns the cached `PaginatedResult<TDto[]>`
   directly.
5. If the result is `IsFailureAndNoData`, return the failure to the caller.
6. Otherwise return the `PaginatedResult<TDto[]>`.

Collection reads never enter a transaction. The `mapper` delegate (`Func<{Entity}[],
TDto[]>`) is received from the adapter layer, keeping DTO concerns outside the
application layer.

## Implementation

```csharp
using App.UseCases.Helpers;
using Core.Domain.Entities.{Module}.{Entities};
using Core.Library.ResultPattern;

namespace App.UseCases.UseCases.{Module}.{Entities};

internal sealed partial class {Entity}UseCase
{
    public async Task<PaginatedResult<TDto[]>> GetPaginated{Entities}Async<TDto>(
        Guid tenantId,
        int? pageQuery,
        int? pageSizeQuery,
        string? sortByQuery,
        bool? isAscendingQuery,
        string? filterQuery,
        Func<{Entity}[], TDto[]> mapper,
        CancellationToken cancellationToken = default) where TDto : class
    {
        // Validate and set default values for pagination and sorting parameters
        int page = pageQuery < 1 || pageQuery is null ? 1 : pageQuery.Value;
        int size = pageSizeQuery < 1 || pageSizeQuery is null ? 25 : pageSizeQuery.Value;

        string sortBy = string.IsNullOrWhiteSpace(sortByQuery) ? "Id" : sortByQuery;
        bool ascending = isAscendingQuery ?? true;

        // Generate a cache key based on all method parameters
        string cacheKey = CacheKeyHelper.Collection(
            nameof({Entity}),
            nameof(GetPaginated{Entities}Async),
            [tenantId, page, size, sortBy, ascending, filterQuery ?? string.Empty]);

        // Attempt to retrieve the paginated collection from the cache, or call the
        // service method if not cached
        PaginatedResult<TDto[]> retrievalResult = await _cacheProvider.GetPaginatedCollection(
            cacheKey: cacheKey,
            useCache: true,
            serviceCall: async () => await _{entity}Service.GetPaginated{Entities}Async(
                tenantId, page, size, sortBy, ascending,
                filterQuery, mapper, cancellationToken),
            _timeout);

        if (retrievalResult.IsFailureAndNoData)
            return retrievalResult;

        return retrievalResult;
    }
}
```

`CacheKeyHelper.Collection` produces a key prefixed with `collection_`, which allows
`ICacheInvalidationService.InvalidateCollections()` to purge all collection caches for
an entity in a single bulk sweep whenever a write occurs.

# UseCase Patterns — Single

## Logic

1. Build a unique cache key via `CacheKeyHelper.Single`, supplying the entity name and
   all identifier segments that distinguish this record (`tenant`, `{entity}Id`).
2. Call `_cacheProvider.GetSingleAsync`, passing the service fetch as the `serviceMethod`
   delegate, `useCache: true`, the constructed key, and `_timeout`.
3. The cache provider checks the cache first. On a miss it invokes the `serviceMethod`,
   stores the result under the key, and returns it. On a hit it deserialises and returns
   the cached value directly.
4. Return the result (`Result<TDto>`) to the caller unchanged — no transaction needed.

Single-entity reads never enter a transaction. The UseCase is generic on the DTO type
(`TDto`) and receives a `Func<{Entity}, TDto> mapper` from the adapter layer, keeping
the mapping concern out of the application layer.

## Implementation

```csharp
using App.UseCases.Helpers;
using Core.Domain.Entities.{Module}.{Entities};
using Core.Library.ResultPattern;

namespace App.UseCases.UseCases.{Module}.{Entities};

internal sealed partial class {Entity}UseCase
{
    public async Task<Result<TDto>> Get{Entity}ByIdAsync<TDto>(
        Guid tenantId,
        Guid {entity}Id,
        Func<{Entity}, TDto> mapper,
        CancellationToken cancellationToken = default) where TDto : class
    {
        // Create a unique cache key for the {entity} based on tenant ID and {entity} ID
        string cacheKey = CacheKeyHelper.Single(
            nameof({Entity}),
            ("tenant", tenantId),
            ("{entity}", {entity}Id));

        // Attempt to retrieve the {entity} from the cache. If not present, fetch from
        // the service and cache the result.
        Result<TDto> {entity}Result = await _cacheProvider.GetSingleAsync(
            serviceMethod: async () => await _{entity}Service.Get{Entity}ByIdAsync(
                tenantId, {entity}Id, mapper, cancellationToken),
            useCache: true,
            cacheKey: cacheKey,
            _timeout);

        return {entity}Result;
    }
}
```

`CacheKeyHelper.Single` produces a deterministic, lowercase key in the format
`{entity}:tenant({tenantId}):{entity}({entityId})`. The same key format is used by the
service's cache invalidation step, ensuring that a write always busts the exact key that
reads look up.
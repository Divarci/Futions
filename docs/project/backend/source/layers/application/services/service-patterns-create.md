# Service Patterns — Create

## Logic

1. Call the domain factory method (`{Entity}.Create(createModel)`) to validate inputs and
   construct the entity. If this returns `IsFailureAndNoData`, return the failure immediately.
2. Persist the new entity via `_repository.CreateAsync(entity, ct)`. If this fails,
   return the failure.
3. Invalidate the single-entity cache key and all collection keys via
   `ICacheInvalidationService`.
4. Return the domain factory result (`Result<{Entity}>`).

## Implementation

```csharp
using Core.Domain.Entities.{Module}.{Entities};
using Core.Domain.Entities.{Module}.{Entities}.Models;
using Core.Library.ResultPattern;

namespace App.Services.Features.{Module}.{Entities};

internal sealed partial class {Entity}Service
{
    public async Task<Result<{Entity}>> Create{Entity}Async(
        {Entity}CreateModel createModel,
        Func<string, (string Label, object Value)[], string> cacheKeyBuilder,
        CancellationToken cancellationToken = default)
    {
        // Create {Entity} from the create model — domain validates inputs.
        Result<{Entity}> createResult = {Entity}.Create(createModel);

        if (createResult.IsFailureAndNoData)
            return createResult;

        // Persist the new entity to the database (change tracker only).
        Result<{Entity}> persistResult = await _{entity}Repository
            .CreateAsync(createResult.Data!, cancellationToken);

        if (persistResult.IsFailureAndNoData)
            return persistResult;

        // Invalidate the single-entity cache entry and all collection caches.
        string cacheKey = cacheKeyBuilder(
            nameof({Entity}),
            [("tenant", createModel.TenantId), ("{entity}", createResult.Data.Id)]);

        await _cacheInvalidationService.InvalidateEntity(cacheKey);
        await _cacheInvalidationService.InvalidateCollections();

        return createResult;
    }
}
```

Return type is always `Result<{Entity}>` — the created entity is returned so the UseCase
layer can pass it to the audit log step without a second DB round-trip.

The `cacheKeyBuilder` delegate is received from the UseCase caller. The service provides
the entity name and the key-value pairs that uniquely identify the new record.

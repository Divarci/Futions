# Service Patterns — Update

## Logic

1. Fetch the entity by ID and tenant ID via the repository. If not found (`IsFailureAndNoData`),
   return the failure immediately.
2. For each updatable field in `updateModel`, check if a new value was supplied. If so,
   call the corresponding domain method (`entity.Update{Field}(value)`). Domain methods
   validate the input and return `Result`. Return immediately on failure.
3. Register the entity as modified via `_repository.Update(entity)` (synchronous, change
   tracker only).
4. Invalidate the single-entity cache key and all collection keys.
5. Return `Result.Success` with `HttpStatusCode.NoContent`.

## Implementation

```csharp
using Core.Domain.Entities.{Module}.{Entities};
using Core.Domain.Entities.{Module}.{Entities}.Models;
using Core.Library.ResultPattern;
using System.Net;

namespace App.Services.Features.{Module}.{Entities};

internal sealed partial class {Entity}Service
{
    public async Task<Result> Update{Entity}Async(
        {Entity}UpdateModel updateModel,
        Func<string, (string Label, object Value)[], string> cacheKeyBuilder,
        CancellationToken cancellationToken = default)
    {
        // Fetch the entity — returns NotFound if missing.
        Result<{Entity}> entityResult = await _{entity}Repository
            .GetByIdAsync(updateModel.{Entity}Id, updateModel.TenantId, cancellationToken);

        if (entityResult.IsFailureAndNoData)
            return entityResult;

        {Entity} {entity} = entityResult.Data;

        // Apply each field conditionally — only call domain method when a new value is supplied.
        if (!string.IsNullOrWhiteSpace(updateModel.{Field}))
        {
            Result update{Field}Result = {entity}.Update{Field}(updateModel.{Field});

            if (update{Field}Result.IsFailure)
                return update{Field}Result;
        }

        // Register entity as modified (change tracker only — no SaveChangesAsync).
        _{entity}Repository.Update({entity});

        // Invalidate the single-entity cache entry and all collection caches.
        string cacheKey = cacheKeyBuilder(
            nameof({Entity}),
            [("tenant", updateModel.TenantId), ("{entity}", updateModel.{Entity}Id)]);

        await _cacheInvalidationService.InvalidateEntity(cacheKey);
        await _cacheInvalidationService.InvalidateCollections();

        return Result.Success(
            message: "{Entity} updated successfully.",
            statusCode: HttpStatusCode.OK);
    }
}
```

Return type is `Result` (non-generic) — the caller does not need the updated object back.

Each field update is conditional. The service checks whether the update model supplies a
new value before calling the domain method. Unchanged fields are skipped entirely, making
the update idempotent for unmodified properties.

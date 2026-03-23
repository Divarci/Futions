# Service Patterns — Delete

## Logic

1. Fetch the entity by ID and tenant ID. If not found, return the failure.
2. Run any pre-deletion business checks (e.g. verify the entity has no dependent
   records that would be orphaned). Each check calls its own repository method and
   returns a failure result if the precondition is not met.
3. Register the entity for removal via `_repository.Delete(entity)` (change tracker only).
4. Invalidate the single-entity cache key and all collection keys.
5. Return `Result.Success` with `HttpStatusCode.NoContent`.

## Implementation

```csharp
using Core.Domain.Entities.{Module}.{Entities};
using Core.Library.ResultPattern;
using System.Net;

namespace App.Services.Features.{Module}.{Entities};

internal sealed partial class {Entity}Service
{
    public async Task<Result> Delete{Entity}Async(
        Guid tenantId,
        Guid {entity}Id,
        Func<string, (string Label, object Value)[], string> cacheKeyBuilder,
        CancellationToken cancellationToken = default)
    {
        // Fetch the entity — returns NotFound if missing.
        Result<{Entity}> entityResult = await _{entity}Repository
            .GetByIdAsync({entity}Id, tenantId, cancellationToken);

        if (entityResult.IsFailureAndNoData)
            return entityResult;

        // Pre-deletion check — verify no dependent records exist.
        Result<bool> hasRelatedResult = await _{related}Repository
            .Has{Related}Async(tenantId, {entity}Id, cancellationToken);

        if (hasRelatedResult.IsFailureAndNoData)
            return hasRelatedResult;

        if (hasRelatedResult.Data!)
            return Result.Failure(
                message: "Cannot delete {entity} with associated {related}. Remove them first.",
                statusCode: HttpStatusCode.BadRequest);

        // Register entity for removal (change tracker only).
        _{entity}Repository.Delete(entityResult.Data);

        // Invalidate the single-entity cache entry and all collection caches.
        string cacheKey = cacheKeyBuilder(
            nameof({Entity}),
            [("tenant", tenantId), ("{entity}", {entity}Id)]);

        await _cacheInvalidationService.InvalidateEntity(cacheKey);
        await _cacheInvalidationService.InvalidateCollections();

        return Result.Success(
            message: "{Entity} deleted successfully.",
            statusCode: HttpStatusCode.OK);
    }
}
```

Return type is `Result` (non-generic). The pre-deletion check block is optional — include
it when the entity has dependents. For entities implementing `IHaveSoftDelete`, replace
`_{entity}Repository.Delete(entity)` with `entity.SoftDelete()` +
`_{entity}Repository.Update(entity)`.
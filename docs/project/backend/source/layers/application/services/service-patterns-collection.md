# Service Patterns — Collection

## Logic

1. Call the entity-specific paginated repository method (e.g.
   `_repository.GetPaginated{Entities}Async(...)`) with all pagination, sort, and filter
   parameters. If the result is `IsFailure`, return `PaginatedResult<TDto[]>.Failure`.
2. Call `_repository.CountAsync(tenantId, ct)` to get the total record count for
   pagination metadata. If this fails, return `PaginatedResult<TDto[]>.Failure`.
3. Apply the `mapper` delegate to the entity array.
4. Return `PaginatedResult<TDto[]>.Success` with data, pagination parameters, and a
   computed `pageCount` (`(int)Math.Ceiling((double)totalCount / pageSize)`).

## Implementation

```csharp
using Core.Domain.Entities.{Module}.{Entities};
using Core.Library.ResultPattern;

namespace App.Services.Features.{Module}.{Entities};

internal sealed partial class {Entity}Service
{
    public async Task<PaginatedResult<TDto[]>> GetPaginated{Entities}Async<TDto>(
        Guid tenantId,
        int page,
        int pageSize,
        string sortBy,
        bool isAscending,
        string? filterQuery,
        Func<{Entity}[], TDto[]> mapper,
        CancellationToken cancellationToken = default) where TDto : class
    {
        // Fetch the page of entities from the repository.
        Result<{Entity}[]> entityResult = await _{entity}Repository
            .GetPaginated{Entities}Async(
                tenantId, page, pageSize, sortBy, isAscending, filterQuery, cancellationToken);

        if (entityResult.IsFailure)
            return PaginatedResult<TDto[]>.Failure(
                message: entityResult.Message,
                statusCode: entityResult.StatusCode);

        // Fetch total count for pagination metadata.
        Result<int> totalCountResult = await _{entity}Repository
            .CountAsync(tenantId, cancellationToken);

        if (totalCountResult.IsFailure)
            return PaginatedResult<TDto[]>.Failure(
                message: totalCountResult.Message,
                statusCode: totalCountResult.StatusCode);

        return PaginatedResult<TDto[]>.Success(
            message: "List retrieved successfully",
            data: mapper(entityResult.Data ?? []),
            pageNumber: page,
            pageSize: pageSize,
            totalCount: totalCountResult.Data,
            pageCount: (int)Math.Ceiling((double)totalCountResult.Data / pageSize));
    }
}
```

Parameters arrive validated and defaulted — nullables are resolved by the UseCase layer
before the service is called. The method is generic on `TDto` for the same reason as the
Single pattern: no Adapter-layer DTO types appear here.

Paginated reads do not interact with the cache. Read-through caching is handled by the
UseCase layer via `ICacheProvider.GetPaginatedCollection`.
# Service Patterns — Single

## Logic

1. Call `_repository.GetByIdAsync(entityId, tenantId, ct)`. If the result is
   `IsFailureAndNoData` (not found), propagate the failure as `Result<TDto>.Failure`.
2. Apply the `mapper` delegate to transform the domain entity to the requested DTO type.
3. Return `Result<TDto>.Success` with the mapped data and the repository's status code.

## Implementation

```csharp
using Core.Domain.Entities.{Module}.{Entities};
using Core.Library.ResultPattern;

namespace App.Services.Features.{Module}.{Entities};

internal sealed partial class {Entity}Service
{
    public async Task<Result<TDto>> Get{Entity}ByIdAsync<TDto>(
        Guid tenantId,
        Guid {entity}Id,
        Func<{Entity}, TDto> mapper,
        CancellationToken cancellationToken = default) where TDto : class
    {
        Result<{Entity}> entityResult = await _{entity}Repository
            .GetByIdAsync({entity}Id, tenantId, cancellationToken);

        if (entityResult.IsFailureAndNoData)
            return Result<TDto>.Failure(
                message: entityResult.Message,
                statusCode: entityResult.StatusCode);

        return Result<TDto>.Success(
            message: entityResult.Message,
            data: mapper(entityResult.Data),
            statusCode: entityResult.StatusCode);
    }
}
```

The method is generic on `TDto`. The service never declares a concrete DTO type — it
receives a `Func<{Entity}, TDto>` from the caller and applies it. This keeps the service
layer free of Adapter-layer DTO dependencies.

Single reads do not interact with the cache. Read-through caching is handled by the
UseCase layer via `ICacheProvider.GetSingleAsync`.

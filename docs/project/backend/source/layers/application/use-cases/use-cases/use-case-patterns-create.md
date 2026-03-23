# UseCase Patterns — Create

## Logic

1. Enter a database transaction via `_unitOfWork.ExecuteTransactionAsync<{Entity}>`.
2. Call `_{entity}Service.Create{Entity}Async(createModel, CacheKeyHelper.Single, ct)`.
   If the result is `IsFailureAndNoData`, return the failure — the transaction rolls back.
3. Call `_auditLogService.CreateAsync(entityId, message, auditStamp, ct)` to record the
   creation event. Audit log failure is **non-fatal**: log a warning with a generated
   trace ID and continue.
4. Return the entity creation result (`Result<{Entity}>`). The created entity is returned
   so the adapter layer can map it to a response without a second database round-trip.

The service receives `CacheKeyHelper.Single` as a delegate. The service uses it to build
and invalidate the single-entity cache key after persisting the new record. Collection
cache invalidation is also triggered by the service.

## Implementation

```csharp
using App.UseCases.Helpers;
using Core.Domain.Entities.Auditing.AuditLogs;
using Core.Domain.Entities.{Module}.{Entities};
using Core.Domain.Entities.{Module}.{Entities}.Models;
using Core.Domain.ValueObjects.AuditStampValueObject;
using Core.Library.ResultPattern;
using Microsoft.Extensions.Logging;

namespace App.UseCases.UseCases.{Module}.{Entities};

internal sealed partial class {Entity}UseCase
{
    public async Task<Result<{Entity}>> Create{Entity}Async(
        {Entity}CreateModel createModel,
        AuditStampCreateModel auditStampCreateModel,
        CancellationToken cancellationToken = default)
    {
        return await _unitOfWork.ExecuteTransactionAsync(async () =>
        {
            // Create {entity}
            Result<{Entity}> {entity}CreateResult = await _{entity}Service
                .Create{Entity}Async(createModel, CacheKeyHelper.Single, cancellationToken);

            if ({entity}CreateResult.IsFailureAndNoData)
                return {entity}CreateResult;

            // Create audit log
            Result<AuditLog> auditLogCreateResult = await _auditLogService
                .CreateAsync(
                    {entity}CreateResult.Data.Id,
                    $"{Entity} created with name: {{entity}CreateResult.Data.Name} by {auditStampCreateModel.Username}",
                    auditStampCreateModel,
                    cancellationToken);

            if (auditLogCreateResult.IsFailureAndNoData)
            {
                string traceId = Guid.NewGuid().ToString();
                _logger.LogWarning(
                    "Audit log creation failed for {Entity} {{EntityId}}. {Message} | TraceId: {TraceId}",
                    {entity}CreateResult.Data.Id,
                    auditLogCreateResult.Message,
                    traceId);
            }

            return {entity}CreateResult;
        }, cancellationToken);
    }
}
```

`ExecuteTransactionAsync<{Entity}>` is the generic overload — it infers the return type
from the lambda and wraps the entire block in a database transaction. Any failure
returned by the service causes the transaction to roll back automatically.

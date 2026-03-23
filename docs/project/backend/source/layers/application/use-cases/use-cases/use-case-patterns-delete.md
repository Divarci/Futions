# UseCase Patterns — Delete

## Logic

1. Enter a database transaction via `_unitOfWork.ExecuteTransactionAsync` (non-generic —
   returns `Result`).
2. Call `_{entity}Service.Delete{Entity}Async(tenantId, entityId, CacheKeyHelper.Single, ct)`.
   If the result is `IsFailure`, return the failure — the transaction rolls back.
3. Call `_auditLogService.CreateAsync(entityId, message, auditStamp, ct)` to record the
   deletion event. Audit log failure is **non-fatal**: log a warning with a trace ID and
   continue.
4. Return the service delete result (`Result`).

Delete differs from Update in that no model object is passed — only the identifiers
(`tenantId` and `entityId`) are needed. The `tenantId` is included to enforce tenant
isolation at the query level inside the service; the service will not find and delete
records that do not belong to the requesting tenant.

## Implementation

```csharp
using App.UseCases.Helpers;
using Core.Domain.Entities.Auditing.AuditLogs;
using Core.Domain.ValueObjects.AuditStampValueObject;
using Core.Library.ResultPattern;
using Microsoft.Extensions.Logging;

namespace App.UseCases.UseCases.{Module}.{Entities};

internal sealed partial class {Entity}UseCase
{
    public async Task<Result> Delete{Entity}Async(
        Guid tenantId,
        Guid {entity}Id,
        AuditStampCreateModel auditStampCreateModel,
        CancellationToken cancellationToken = default)
    {
        return await _unitOfWork.ExecuteTransactionAsync(async () =>
        {
            // Delete {entity}
            Result {entity}DeleteResult = await _{entity}Service
                .Delete{Entity}Async(tenantId, {entity}Id, CacheKeyHelper.Single, cancellationToken);

            if ({entity}DeleteResult.IsFailure)
                return {entity}DeleteResult;

            // Create audit log
            Result<AuditLog> auditLogCreateResult = await _auditLogService
                .CreateAsync(
                    {entity}Id,
                    $"{Entity} with ID {{entity}Id} has been deleted by {auditStampCreateModel.Username}.",
                    auditStampCreateModel,
                    cancellationToken);

            if (auditLogCreateResult.IsFailureAndNoData)
            {
                string traceId = Guid.NewGuid().ToString();
                _logger.LogWarning(
                    "Audit log creation failed for {Entity} {{EntityId}}. {Message} | TraceId: {TraceId}",
                    {entity}Id,
                    auditLogCreateResult.Message,
                    traceId);
            }

            return {entity}DeleteResult;
        }, cancellationToken);
    }
}
```
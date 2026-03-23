# UseCase Patterns — Update

## Logic

1. Enter a database transaction via `_unitOfWork.ExecuteTransactionAsync` (non-generic —
   returns `Result`, not `Result<{Entity}>`).
2. Call `_{entity}Service.Update{Entity}Async(updateModel, CacheKeyHelper.Single, ct)`.
   If the result is `IsFailure`, return the failure — the transaction rolls back.
3. Call `_auditLogService.CreateAsync(entityId, message, auditStamp, ct)` to record the
   update event. Audit log failure is **non-fatal**: log a warning with a trace ID and
   continue.
4. Return the service update result (`Result`).

Update operations do not return the modified entity. The caller already knows the identity
of the record being changed, so returning a plain `Result` is sufficient and avoids an
unnecessary re-fetch or mapping step.

The check after the service call uses `IsFailure` (not `IsFailureAndNoData`) because the
non-generic `Result` type carries no data — `IsFailure` is the correct sentinel here.

## Implementation

```csharp
using App.UseCases.Helpers;
using Core.Domain.Entities.Auditing.AuditLogs;
using Core.Domain.Entities.{Module}.{Entities}.Models;
using Core.Domain.ValueObjects.AuditStampValueObject;
using Core.Library.ResultPattern;
using Microsoft.Extensions.Logging;

namespace App.UseCases.UseCases.{Module}.{Entities};

internal sealed partial class {Entity}UseCase
{
    public async Task<Result> Update{Entity}Async(
        {Entity}UpdateModel updateModel,
        AuditStampCreateModel auditStampCreateModel,
        CancellationToken cancellationToken = default)
    {
        return await _unitOfWork.ExecuteTransactionAsync(async () =>
        {
            // Update {entity}
            Result {entity}UpdateResult = await _{entity}Service
                .Update{Entity}Async(updateModel, CacheKeyHelper.Single, cancellationToken);

            if ({entity}UpdateResult.IsFailure)
                return {entity}UpdateResult;

            // Create audit log
            Result<AuditLog> auditLogCreateResult = await _auditLogService
                .CreateAsync(
                    updateModel.{Entity}Id,
                    $"{Entity} with ID {updateModel.{Entity}Id} has been updated by {auditStampCreateModel.Username}.",
                    auditStampCreateModel,
                    cancellationToken);

            if (auditLogCreateResult.IsFailureAndNoData)
            {
                string traceId = Guid.NewGuid().ToString();
                _logger.LogWarning(
                    "Audit log creation failed for {Entity} {{EntityId}}. {Message} | TraceId: {TraceId}",
                    updateModel.{Entity}Id,
                    auditLogCreateResult.Message,
                    traceId);
            }

            return {entity}UpdateResult;
        }, cancellationToken);
    }
}
```

using Core.Domain.Entities.Auditing.AuditLogs;
using Core.Domain.ValueObjects.AuditStampValueObject;
using Core.Library.ResultPattern;

namespace App.Services.Features.Organisations.Companies;

internal sealed partial class AuditLogService
{
    public async Task<Result<AuditLog>> CreateAsync(
        Guid entityId,
        string description,
        AuditStampCreateModel createModel,
        CancellationToken cancellationToken = default)
    {
        // Create the AuditLog entity using the factory method.
        Result<AuditLog> auditLogResult = AuditLog.Create(createModel, entityId, description);
        if (auditLogResult.IsFailureAndNoData)
            return auditLogResult;

        // Persist the AuditLog entity to the database using the repository.
        Result<AuditLog> persistResult = await _auditLogRepository.CreateAsync(auditLogResult.Data!, cancellationToken);

        if (persistResult.IsFailureAndNoData)
            return persistResult;

        // Create cache key
        string cacheKey = $"{nameof(AuditLog)}:tenant({createModel.TenantId}):id({auditLogResult.Data.Id})";

        // Invalidate the cache for the newly created company and the collections that may include it.
        await _cacheInvalidationService.InvalidateEntity(cacheKey);
        await _cacheInvalidationService.InvalidateCollections();

        return auditLogResult;
    }
}
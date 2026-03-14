using Core.Domain.Entities.Auditing.AuditLogs;
using Core.Domain.Entities.System.AuditLogs.Models;
using Core.Library.ResultPattern;

namespace App.Services.Features.Organisations.Companies;

internal sealed partial class AuditLogService
{
    public async Task<Result<AuditLog>> CreateAsync(
        Guid tenantId,
        Guid entityId,
        string description,
        AuditLogCreateModel createModel,
        CancellationToken cancellationToken = default)
    {
        // Create the AuditLog entity using the factory method.
        Result<AuditLog> auditLogResult = AuditLog.Create(createModel, entityId, description);

        if (auditLogResult.IsFailureAndNoData)
            return auditLogResult;

        // Persist the AuditLog entity to the database using the repository.
        await _auditLogRepository.CreateAsync(auditLogResult.Data!, cancellationToken);

        // Create cache key
        string cacheKey = $"{nameof(AuditLog)}:tenant({tenantId}):id({auditLogResult.Data.Id})";

        // Invalidate the cache for the newly created company and the collections that may include it.
        await _cacheInvalidationService.InvalidateEntity(cacheKey);
        await _cacheInvalidationService.InvalidateCollections();

        return auditLogResult;
    }
}
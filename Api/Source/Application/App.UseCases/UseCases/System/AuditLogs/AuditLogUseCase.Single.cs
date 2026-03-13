using Core.Domain.Entities.Auditing.AuditLogs;
using Core.Library.ResultPattern;

namespace App.UseCases.UseCases.System.AuditLogs;

internal sealed partial class AuditLogUseCase
{
    public async Task<Result<AuditLog>> GetByIdAsync(
        Guid tenantId,
        Guid id,
        CancellationToken cancellationToken = default)
    {
        // Create a unique cache key for the audit log based on tenant ID and audit log ID
        string cacheKey = $"{nameof(AuditLog)}:tenant({tenantId}):id({id})";

        // Attempt to retrieve the audit log from the cache. If it's not present, fetch it from the service and cache the result.
        Result<AuditLog> auditLogResult = await _cacheProvider.GetSingleAsync(
            serviceMethod: async () => await _auditLogService.GetByIdAsync(
                tenantId, id, cancellationToken),
            useCache: true,
            cacheKey: cacheKey,
            _timeout);

        return auditLogResult;
    }
}

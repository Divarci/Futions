using App.UseCases.Helpers;
using Core.Domain.Entities.System.AuditLogs;
using Core.Library.ResultPattern;

namespace App.UseCases.UseCases.System.AuditLogs;

internal sealed partial class AuditLogUseCase
{
    public async Task<Result<TDto>> GetByIdAsync<TDto>(
        Guid tenantId,
        Guid id,
        Func<AuditLog, TDto> mapper,
        CancellationToken cancellationToken = default) where TDto : class
    {
        // Create a unique cache key for the audit log based on tenant ID and audit log ID
        string cacheKey = CacheKeyHelper.Single(nameof(AuditLog), ("tenant", tenantId), ("id", id));

        // Attempt to retrieve the audit log from the cache. If it's not present, fetch it from the service and cache the result.
        Result<TDto> auditLogResult = await _cacheProvider.GetSingleAsync(
            serviceMethod: async () => await _auditLogService.GetByIdAsync(
                tenantId, id, mapper, cancellationToken),
            useCache: true,
            cacheKey: cacheKey,
            _timeout);

        return auditLogResult;
    }
}

using App.UseCases.Helpers;
using Core.Domain.Entities.Auditing.AuditLogs;
using Core.Library.ResultPattern;

namespace App.UseCases.UseCases.System.AuditLogs;

internal sealed partial class AuditLogUseCase
{
    public async Task<PaginatedResult<AuditLog[]>> GetPaginatedAsync(
        Guid tenantId,
        int? pageQuery,
        int? pageSizeQuery,
        string? sortByQuery,
        bool? isAscendingQuery,
        string? filterQuery,
        CancellationToken cancellationToken = default)
    {
        // Validate and set default values for pagination and sorting parameters
        int page = pageQuery < 1 || pageQuery is null ? 1 : pageQuery.Value;
        int size = pageSizeQuery < 1 || pageSizeQuery is null ? 25 : pageSizeQuery.Value;

        string sortBy = string.IsNullOrWhiteSpace(sortByQuery) ? "Id" : sortByQuery;
        bool ascending = isAscendingQuery ?? true;

        // Generate a cache key based on the method parameters
        string cacheKey = CacheKeyHelper.Collection(
            nameof(AuditLog),
            nameof(GetPaginatedAsync),
            [tenantId, page, size, sortBy, ascending, filterQuery ?? string.Empty]);

        // Attempt to retrieve the paginated collection from the cache, or call the service method if not cached
        PaginatedResult<AuditLog[]> retrievalResult = await _cacheProvider.GetPaginatedCollection(
            cacheKey: cacheKey,
            useCache: true,
            serviceCall: async () => await _auditLogService.GetPaginatedAsync(
                tenantId, page, size, sortBy, ascending,
                filterQuery, cancellationToken),
            _timeout);

        if (retrievalResult.IsFailureAndNoData)
            return retrievalResult;

        return retrievalResult;
    }
}

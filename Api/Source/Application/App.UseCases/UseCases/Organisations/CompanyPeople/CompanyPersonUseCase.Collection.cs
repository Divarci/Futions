using App.UseCases.Helpers;
using Core.Domain.Entities.Organisations.CompanyPeople;
using Core.Library.ResultPattern;

namespace App.UseCases.UseCases.Organisations.CompanyPeople;

internal sealed partial class CompanyPersonUseCase
{
    public async Task<PaginatedResult<CompanyPerson[]>> GetPaginatedAsync(
        Guid tenantId,
        int? pageQuery,
        int? pageSizeQuery,
        string? sortByQuery,
        bool? isAscendingQuery,
        string? filterQuery,
        CancellationToken cancellationToken = default)
    {
        // Validate and set default values for pagination and sorting parameters.
        int page = pageQuery < 1 || pageQuery is null ? 1 : pageQuery.Value;
        int size = pageSizeQuery < 1 || pageSizeQuery is null ? 25 : pageSizeQuery.Value;

        string sortBy = string.IsNullOrWhiteSpace(sortByQuery) ? "Id" : sortByQuery;
        bool ascending = isAscendingQuery ?? true;

        // Generate a cache key based on the method parameters.
        string cacheKey = CacheKeyHelper.Collection(
            nameof(CompanyPerson),
            nameof(GetPaginatedAsync),
            [tenantId, page, size, sortBy, ascending, filterQuery ?? string.Empty]);

        // Attempt to retrieve the paginated collection from the cache, or call the service method if not cached.
        PaginatedResult<CompanyPerson[]> retrievalResult = await _cacheProvider.GetPaginatedCollection(
            cacheKey: cacheKey,
            useCache: true,
            serviceCall: async () => await _companyPersonService.GetPaginatedAsync(
                tenantId, page, size, sortBy, ascending,
                filterQuery, cancellationToken),
            _timeout);

        if (retrievalResult.IsFailureAndNoData)
            return retrievalResult;

        return retrievalResult;
    }
}

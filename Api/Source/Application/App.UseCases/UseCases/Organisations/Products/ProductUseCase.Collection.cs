using App.UseCases.Helpers;
using Core.Domain.Entities.Organisations.Products;
using Core.Library.ResultPattern;

namespace App.UseCases.UseCases.Organisations.Products;

internal sealed partial class ProductUseCase
{
    public async Task<PaginatedResult<TDto[]>> GetPaginatedAsync<TDto>(
        Guid tenantId,
        Guid companyId,
        int? pageQuery,
        int? pageSizeQuery,
        string? sortByQuery,
        bool? isAscendingQuery,
        string? filterQuery,
        Func<Product[], TDto[]> mapper,
        CancellationToken cancellationToken = default) where TDto : class
    {
        // Validate and set default values for pagination and sorting parameters.
        int page = pageQuery < 1 || pageQuery is null ? 1 : pageQuery.Value;
        int size = pageSizeQuery < 1 || pageSizeQuery is null ? 25 : pageSizeQuery.Value;

        string sortBy = string.IsNullOrWhiteSpace(sortByQuery) ? "Id" : sortByQuery;
        bool ascending = isAscendingQuery ?? true;

        // Generate a cache key based on the method parameters.
        string cacheKey = CacheKeyHelper.Collection(
            nameof(Product),
            nameof(GetPaginatedAsync),
            [tenantId, page, size, sortBy, ascending, filterQuery ?? string.Empty]);

        // Attempt to retrieve the paginated collection from the cache, or call the service method if not cached.
        PaginatedResult<TDto[]> retrievalResult = await _cacheProvider.GetPaginatedCollection(
            cacheKey: cacheKey,
            useCache: true,
            serviceCall: async () => await _productService.GetPaginatedAsync(
                tenantId, companyId, page, size, sortBy, ascending,
                filterQuery, mapper, cancellationToken),
            _timeout);

        if (retrievalResult.IsFailureAndNoData)
            return retrievalResult;

        return retrievalResult;
    }
}

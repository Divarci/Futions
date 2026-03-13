using App.UseCases.Helpers;
using Core.Domain.Entities.Organisations.People;
using Core.Library.ResultPattern;

namespace App.UseCases.UseCases.Organisations.People;

internal sealed partial class PersonUseCase
{
    public async Task<PaginatedResult<TDto[]>> GetPaginatedAsync<TDto>(
        Guid tenantId,
        int? pageQuery,
        int? pageSizeQuery,
        string? sortByQuery,
        bool? isAscendingQuery,
        string? filterQuery,
        Func<Person[], TDto[]> mapper,
        CancellationToken cancellationToken = default) where TDto : class
    {
        int page = pageQuery < 1 || pageQuery is null ? 1 : pageQuery.Value;
        int size = pageSizeQuery < 1 || pageSizeQuery is null ? 25 : pageSizeQuery.Value;

        string sortBy = string.IsNullOrWhiteSpace(sortByQuery) ? "Id" : sortByQuery;
        bool ascending = isAscendingQuery ?? true;

        string cacheKey = CacheKeyHelper.Collection(
            nameof(Person),
            nameof(GetPaginatedAsync),
            [tenantId, page, size, sortBy, ascending, filterQuery ?? string.Empty]);

        PaginatedResult<TDto[]> retrievalResult = await _cacheProvider.GetPaginatedCollection(
            cacheKey: cacheKey,
            useCache: true,
            serviceCall: async () => await _personService.GetPaginatedAsync(
                tenantId, page, size, sortBy, ascending,
                filterQuery, mapper, cancellationToken),
            new(1, 0, 0));

        if (retrievalResult.IsFailureAndNoData)
            return retrievalResult;

        return retrievalResult;
    }
}

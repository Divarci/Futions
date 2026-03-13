using Core.Domain.Entities.Organisations.CompanyPeople;
using Core.Library.ResultPattern;

namespace App.Services.Features.Organisations.Companies;

internal sealed partial class CompanyPeopleService
{
    public async Task<PaginatedResult<TDto[]>> GetPaginatedAsync<TDto>(
        Guid tenantId,
        int page,
        int pageSize,
        string sortBy,
        bool isAscending,
        string? filterQuery,
        Func<CompanyPerson[], TDto[]> mapper,
        CancellationToken cancellationToken = default) where TDto : class
    {
        Result<CompanyPerson[]> entityResult = await _companyPersonRepository
            .GetPaginatedAsync(tenantId, page, pageSize, sortBy, isAscending, filterQuery, cancellationToken);

        if (entityResult.IsFailure)
            return PaginatedResult<TDto[]>.Failure(
                message: entityResult.Message,
                statusCode: entityResult.StatusCode);

        Result<int> totalCountResult = await _companyPersonRepository
            .CountAsync(cancellationToken);

        if (totalCountResult.IsFailure)
            return PaginatedResult<TDto[]>.Failure(
                message: totalCountResult.Message,
                statusCode: totalCountResult.StatusCode);

        return PaginatedResult<TDto[]>.Success(
            message: "List retrieved successfully",
            data: mapper(entityResult.Data ?? []),
            pageNumber: page,
            pageSize: pageSize,
            totalCount: totalCountResult.Data,
            pageCount: (int)Math.Ceiling((double)totalCountResult.Data / pageSize));
    }
}
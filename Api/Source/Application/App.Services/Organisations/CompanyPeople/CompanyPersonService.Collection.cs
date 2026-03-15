using Core.Domain.Entities.Organisations.CompanyPeople;
using Core.Library.ResultPattern;

namespace App.Services.Features.Organisations.Companies;

internal sealed partial class CompanyPersonService
{
    public async Task<PaginatedResult<TDto[]>> GetPaginatedCompanyPeopleAsync<TDto>(
        Guid tenantId,
        Guid companyId,
        int page,
        int pageSize,
        string sortBy,
        bool isAscending,
        string? filterQuery,
        Func<CompanyPerson[], TDto[]> mapper,
        CancellationToken cancellationToken = default) where TDto : class
    {
        // Get paginated list of company people for tenant
        Result<CompanyPerson[]> entityResult = await _companyPersonRepository
            .GetPaginatedCompanyPeopleAsync(tenantId, companyId, page, pageSize, sortBy, isAscending, filterQuery, cancellationToken);

        if (entityResult.IsFailure)
            return PaginatedResult<TDto[]>.Failure(
                message: entityResult.Message,
                statusCode: entityResult.StatusCode);

        // Get total count of company people for tenant
        Result<int> totalCountResult = await _companyPersonRepository
            .CountCompanyPeopleAsync(tenantId, companyId,cancellationToken);

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
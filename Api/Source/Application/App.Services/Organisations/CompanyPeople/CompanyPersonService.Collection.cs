using Core.Domain.Entities.Organisations.CompanyPeople;
using Core.Library.ResultPattern;

namespace App.Services.Features.Organisations.Companies;

internal sealed partial class CompanyPersonService
{
    public async Task<PaginatedResult<CompanyPerson[]>> GetPaginatedAsync(
        Guid tenantId,
        int page,
        int pageSize,
        string sortBy,
        bool isAscending,
        string? filterQuery,
        CancellationToken cancellationToken = default)
    {
        // Get paginated list of company people for tenant
        Result<CompanyPerson[]> entityResult = await _companyPersonRepository
            .GetPaginatedAsync(tenantId, page, pageSize, sortBy, isAscending, filterQuery, cancellationToken);

        if (entityResult.IsFailure)
            return PaginatedResult<CompanyPerson[]>.Failure(
                message: entityResult.Message,
                statusCode: entityResult.StatusCode);

        // Get total count of company people for tenant
        Result<int> totalCountResult = await _companyPersonRepository
            .CountAsync(cancellationToken);

        if (totalCountResult.IsFailure)
            return PaginatedResult<CompanyPerson[]>.Failure(
                message: totalCountResult.Message,
                statusCode: totalCountResult.StatusCode);

        return PaginatedResult<CompanyPerson[]>.Success(
            message: "List retrieved successfully",
            data: entityResult.Data ?? [],
            pageNumber: page,
            pageSize: pageSize,
            totalCount: totalCountResult.Data,
            pageCount: (int)Math.Ceiling((double)totalCountResult.Data / pageSize));
    }
}
using Core.Domain.Entities.Organisations.Companies;
using Core.Library.ResultPattern;

namespace App.Services.Features.Organisations.Companies;

internal sealed partial class CompanyService
{
    public async Task<PaginatedResult<TDto[]>> GetPaginatedAsync<TDto>(
        Guid tenantId, 
        int page, 
        int pageSize, 
        string sortBy, 
        bool ascending, 
        string? filterQuery, 
        Func<Company[], TDto[]> mapper,
        CancellationToken cancellationToken = default) where TDto : class
    {
        // Get paginated list of companies for the tenant
        Result<Company[]> entityResult = await _companyRepository
            .GetPaginatedAsync(tenantId, page, pageSize, sortBy, ascending, filterQuery, cancellationToken);

        if (entityResult.IsFailure)        
            return PaginatedResult<TDto[]>.Failure(
                message: entityResult.Message,
                statusCode: entityResult.StatusCode);

        // Get total count of companies for the tenant (for pagination metadata)
        Result<int> totalCountResult = await _companyRepository
            .CountAsync(tenantId, cancellationToken);

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
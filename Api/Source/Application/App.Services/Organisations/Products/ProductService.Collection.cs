using Core.Domain.Entities.Organisations.Products;
using Core.Library.ResultPattern;

namespace App.Services.Features.Organisations.Companies;

internal sealed partial class ProductService
{
    public async Task<PaginatedResult<Product[]>> GetPaginatedAsync(
        Guid tenantId,
        int page,
        int pageSize,
        string sortBy,
        bool isAscending,
        string? filterQuery,
        CancellationToken cancellationToken = default)
    {
        // Get paginated list of products for the specified tenant
        Result<Product[]> entityResult = await _repository
            .GetPaginatedAsync(tenantId, page, pageSize, sortBy, isAscending, filterQuery, cancellationToken);

        if (entityResult.IsFailure)
            return PaginatedResult<Product[]>.Failure(
                message: entityResult.Message,
                statusCode: entityResult.StatusCode);

        // Get total count of products for the specified tenant
        Result<int> totalCountResult = await _repository
            .CountAsync(tenantId, cancellationToken);

        if (totalCountResult.IsFailure)
            return PaginatedResult<Product[]>.Failure(
                message: totalCountResult.Message,
                statusCode: totalCountResult.StatusCode);

        return PaginatedResult<Product[]>.Success(
            message: "List retrieved successfully",
            data: entityResult.Data ?? [],
            pageNumber: page,
            pageSize: pageSize,
            totalCount: totalCountResult.Data,
            pageCount: (int)Math.Ceiling((double)totalCountResult.Data / pageSize));
    }
}
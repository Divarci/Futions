using Core.Domain.Entities.Organisations.Products;
using Core.Library.ResultPattern;

namespace App.Services.Features.Organisations.Companies;

internal sealed partial class ProductService
{
    public async Task<Result<TDto>> GetCompanyProductByIdAsync<TDto>(
        Guid tenantId,
        Guid companyId,
        Guid productId,
        Func<Product, TDto> mapper,
        CancellationToken cancellationToken = default) where TDto : class
    {
        // Get the product by id and tenant id.
        Result<Product> entityResult = await _repository
            .GetCompanyProductByIdAsync(productId, tenantId, companyId, cancellationToken);

        if (entityResult.IsFailureAndNoData)
            return Result<TDto>.Failure(
                message: entityResult.Message,
                statusCode: entityResult.StatusCode);

        return Result<TDto>.Success(
            message: entityResult.Message,
            data: mapper(entityResult.Data),
            statusCode: entityResult.StatusCode);
    }
}
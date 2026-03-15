using Core.Domain.Entities.Organisations.Products;
using Core.Library.ResultPattern;

namespace App.Services.Features.Organisations.Companies;

internal sealed partial class ProductService
{
    public async Task<Result<TDto>> GetByIdAsync<TDto>(
        Guid tenantId,
        Guid id,
        Func<Product, TDto> mapper,
        CancellationToken cancellationToken = default) where TDto : class
    {
        // Get the product by id and tenant id.
        Result<Product> entityResult = await _repository
            .GetByIdAsync(id, tenantId, cancellationToken);

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
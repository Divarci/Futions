using Core.Domain.Entities.Organisations.Products;
using Core.Library.ResultPattern;

namespace App.UseCases.UseCases.Organisations.Products;

internal sealed partial class ProductUseCase
{
    public async Task<Result<TDto>> GetCompanyProductByIdAsync<TDto>(
        Guid tenantId,
        Guid companyId,
        Guid productId,
        Func<Product, TDto> mapper,
        CancellationToken cancellationToken = default) where TDto : class
    {
        // Create a unique cache key for the product based on tenant ID and product ID.
        string cacheKey = $"{nameof(Product)}:tenant({tenantId}):company({companyId}:product({productId})";

        // Attempt to retrieve the product from the cache. If it's not present, fetch it from the service and cache the result.
        Result<TDto> productResult = await _cacheProvider.GetSingleAsync(
            serviceMethod: async () => await _productService.GetCompanyProductByIdAsync(
                tenantId, companyId, productId, mapper, cancellationToken),
            useCache: true,
            cacheKey: cacheKey,
            _timeout);

        return productResult;
    }
}

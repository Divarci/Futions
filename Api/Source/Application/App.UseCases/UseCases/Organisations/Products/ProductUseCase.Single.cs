using Core.Domain.Entities.Organisations.Products;
using Core.Library.ResultPattern;

namespace App.UseCases.UseCases.Organisations.Products;

internal sealed partial class ProductUseCase
{
    public async Task<Result<Product>> GetByIdAsync(
        Guid tenantId,
        Guid id,
        CancellationToken cancellationToken = default)
    {
        // Create a unique cache key for the product based on tenant ID and product ID.
        string cacheKey = $"{nameof(Product)}:tenant({tenantId}):id({id})";

        // Attempt to retrieve the product from the cache. If it's not present, fetch it from the service and cache the result.
        Result<Product> productResult = await _cacheProvider.GetSingleAsync(
            serviceMethod: async () => await _productService.GetByIdAsync(
                tenantId, id, cancellationToken),
            useCache: true,
            cacheKey: cacheKey,
            _timeout);

        return productResult;
    }
}

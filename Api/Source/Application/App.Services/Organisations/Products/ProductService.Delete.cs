using Core.Domain.Entities.Organisations.Products;
using Core.Library.ResultPattern;

namespace App.Services.Features.Organisations.Companies;

internal sealed partial class ProductService
{
    public async Task<Result> DeleteCompanyProductAsync(
        Guid tenantId,
        Guid companyId,
        Guid productId,
        CancellationToken cancellationToken = default)
    {
        // Check if the product exists.
        Result<Product> productResult = await _repository
            .GetCompanyProductByIdAsync(productId, tenantId, companyId, cancellationToken);

        if (productResult.IsFailureAndNoData)
            return productResult;

        // Soft delete the product.
        Result deleteResult = productResult.Data.SoftDelete();

        // Create cache key.
        string cacheKey = $"{nameof(Product)}:tenant({tenantId}):company({companyId}):product({productId})";

        // Invalidate the cache for the newly created product and the collections that may include it.
        await _cacheInvalidationService.InvalidateEntity(cacheKey);
        await _cacheInvalidationService.InvalidateCollections();

        return deleteResult;
    }
}
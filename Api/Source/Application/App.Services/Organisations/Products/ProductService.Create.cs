using Core.Domain.Entities.Organisations.Products;
using Core.Domain.Entities.Organisations.Products.Models;
using Core.Library.ResultPattern;

namespace App.Services.Features.Organisations.Companies;

internal sealed partial class ProductService
{
    public async Task<Result<Product>> CreateAsync(
        Guid tenantId,
        ProductCreateModel createModel,
        CancellationToken cancellationToken = default)
    {
        // Get Tenant and ensure tenantId is valid and exists.Since this is an example,
        // we will skip this step and assume tenantId is valid and exists.

        // Get Company and ensure companyId is valid and exists.
        Result<bool> companyExistsResult = await _companyRepository
            .ExistsAsync(createModel.CompanyId, tenantId, cancellationToken);

        if (companyExistsResult.IsFailure)
            return Result<Product>.Failure(
                message: companyExistsResult.Message,
                statusCode: companyExistsResult.StatusCode);

        // Create Product entity from the create model.
        Result<Product> productCreateResult = Product.Create(createModel, tenantId);

        if (productCreateResult.IsFailureAndNoData)
            return productCreateResult;

        // Persist the new Product entity to the database.
        await _repository.CreateAsync(productCreateResult.Data, cancellationToken);

        // Create cache key.
        string cacheKey = $"{nameof(Product)}:tenant({tenantId}):id({productCreateResult.Data!.Id})";

        // Invalidate the cache for the newly created product and the collections that may include it.
        await _cacheInvalidationService.InvalidateEntity(cacheKey);
        await _cacheInvalidationService.InvalidateCollections();

        return productCreateResult;
    }
}
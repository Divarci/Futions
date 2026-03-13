using Core.Domain.Entities.Organisations.Products;
using Core.Library.ResultPattern;
using System.Net;

namespace App.Services.Features.Organisations.Companies;

internal sealed partial class ProductService
{
    public async Task<Result> DeleteAsync(
        Guid tenantId,
        Guid id,
        CancellationToken cancellationToken = default)
    {
        // Check if the product exists.
        Result<Product> productResult = await _repository
            .GetByIdAsync(id, tenantId, cancellationToken);

        if (productResult.IsFailureAndNoData)
            return productResult;

        // Soft delete the product.
        _repository.Delete(productResult.Data);

        return Result.Success(
            message: "Product deleted successfully.",
            statusCode: HttpStatusCode.OK);
    }
}
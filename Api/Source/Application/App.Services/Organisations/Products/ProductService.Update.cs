using Core.Domain.Entities.Organisations.Products;
using Core.Domain.Entities.Organisations.Products.Models;
using Core.Library.ResultPattern;
using System.Net;

namespace App.Services.Features.Organisations.Companies;

internal sealed partial class ProductService
{
    public async Task<Result<Product>> UpdateCompanyProductAsync(
        ProductUpdateModel updateModel,
        CancellationToken cancellationToken = default)
    {
        // Retrieve the product to update.
        Result<Product> productResult = await _repository
            .GetCompanyProductByIdAsync(updateModel.ProductId, updateModel.TenantId, updateModel.CompanyId, cancellationToken);

        if (productResult.IsFailureAndNoData)
            return productResult;

        Product product = productResult.Data!;

        // If name is provided, update it; otherwise, keep the existing name.
        if (!string.IsNullOrWhiteSpace(updateModel.Name))
        {
            Result updateNameResult = product
                .UpdateName(updateModel.Name);

            if (updateNameResult.IsFailure)
                return Result<Product>.Failure(
                    message: updateNameResult.Message,
                    statusCode: updateNameResult.StatusCode);   
        }

        // If price is provided, update it; otherwise, keep the existing price.
        if (updateModel.Price is not null)
        {
            Result updatePriceResult = product
                .UpdatePrice(updateModel.Price.Value);

            if (updatePriceResult.IsFailure)
                return Result<Product>.Failure(
                    message: updatePriceResult.Message,
                    statusCode: updatePriceResult.StatusCode);
        }

        // Persist the updated product entity.
        _repository.Update(product);

        return Result<Product>.Success(
            message: "Product updated successfully.",
            data: product,
            statusCode: HttpStatusCode.OK);
    }
}
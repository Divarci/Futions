using Adapter.RestApi.Controllers.VersionOne.Organisations.Products.Models.Requests;
using Adapter.RestApi.Controllers.VersionOne.Organisations.Products.Models.Responses;
using Core.Domain.Entities.Organisations.Products;
using Core.Domain.Entities.Organisations.Products.Models;

namespace Adapter.RestApi.Controllers.VersionOne.Organisations.Products;

internal static class ProductMapper
{
    internal static ProductResponse ToResponse(Product product)
        => new()
        {
            Id = product.Id,
            CompanyId = product.CompanyId,
            Name = product.Name,
            Price = product.Price
        };

    internal static ProductResponse[] ToArrayResponse(Product[] products)
        => [.. products.Select(ToResponse)];

    internal static ProductCreateModel ToCreateModel(CreateProductRequest request, Guid tenantId)
        => new()
        {
            TenantId = tenantId,
            CompanyId = request.CompanyId!.Value,
            Name = request.Name!,
            Price = request.Price!.Value
        };

    internal static ProductUpdateModel ToUpdateModel(UpdateProductRequest request, Guid productId)
        => new()
        {
            ProductId = productId,
            Name = request.Name,
            Price = request.Price
        };
}

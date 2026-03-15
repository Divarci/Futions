using Adapter.RestApi.Controllers.VersionOne.Organisations.Companies.CompanyProducts.Models.Requests;
using Adapter.RestApi.Controllers.VersionOne.Organisations.Companies.CompanyProducts.Models.Responses;
using Core.Domain.Entities.Organisations.Products;
using Core.Domain.Entities.Organisations.Products.Models;

namespace Adapter.RestApi.Controllers.VersionOne.Organisations.Companies.CompanyProducts;

internal static class CompanyProductMapper
{
    internal static CompanyProductResponse ToResponse(Product product)
        => new()
        {
            Id = product.Id,
            CompanyId = product.CompanyId,
            Name = product.Name,
            Price = product.Price
        };

    internal static CompanyProductResponse[] ToArrayResponse(Product[] products)
        => [.. products.Select(ToResponse)];

    internal static ProductCreateModel ToCreateModel(CreateCompanyProductRequest request, Guid tenantId, Guid companyId)
        => new()
        {
            TenantId = tenantId,
            CompanyId = companyId,
            Name = request.Name!,
            Price = request.Price!.Value
        };

    internal static ProductUpdateModel ToUpdateModel(UpdateCompanyProductRequest request,Guid tenantId, Guid companyId, Guid productId)
        => new()
        {
            TenantId = tenantId,
            CompanyId = companyId,
            ProductId = productId,
            Name = request.Name,
            Price = request.Price
        };
}

using Core.Domain.Entities.Organisations.Products;
using Core.Domain.Entities.Organisations.Products.Models;
using Core.Library.ResultPattern;

namespace App.Services.Features.Organisations.Companies;

internal sealed partial class ProductService
{
    public Task<Result<Product>> UpdateAsync(Guid tenantId, ProductUpdateModel updateModel, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
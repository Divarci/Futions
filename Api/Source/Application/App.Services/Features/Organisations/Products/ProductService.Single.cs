using Core.Domain.Entities.Organisations.Products;
using Core.Library.ResultPattern;

namespace App.Services.Features.Organisations.Companies;

internal sealed partial class ProductService
{
    public Task<Result<Product>> GetByIdAsync(Guid tenantId, Guid id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
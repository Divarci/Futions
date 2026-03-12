using Core.Domain.Entities.Organisations.Products;
using Core.Library.ResultPattern;

namespace App.Services.Features.Organisations.Companies;

internal sealed partial class ProductService
{
    public Task<Result<Product[]>> GetPaginatedAsync(Guid tenantId, int page, int pageSize, string sortBy, bool isAscending, string? filter, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
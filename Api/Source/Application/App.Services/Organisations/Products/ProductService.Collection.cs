using Core.Domain.Entities.Organisations.Products;
using Core.Library.ResultPattern;

namespace App.Services.Features.Organisations.Companies;

internal sealed partial class ProductService
{
    public Task<Result<Product[]>> GetPaginatedAsync(Guid tenantId, int? pageQuery, int? pageSizeQuery, string? sortByQuery, bool? isAscendingQuery, string? filterQuery, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
using Core.Domain.Entities.Organisations.Products;
using Core.Library.ResultPattern;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace Infra.Persistence.Repositories.Organisations.Products;

internal sealed partial class ProductRepository
{
    public async Task<Result<int>> CountCompanyProductsAsync(
        Guid tenantId,
        Guid companyId,
        CancellationToken cancellationToken = default)
    {
        int productCount = await _context.Set<Product>()
            .AsNoTracking()
            .CountAsync(x =>
                x.TenantId == tenantId &&
                x.CompanyId == companyId &&
                !x.IsDeleted &&
                !x.Company.IsDeleted, cancellationToken);

        return Result<int>.Success(
            message: "Product count retrieved successfully.",
            data: productCount,
            statusCode: HttpStatusCode.OK);
    }
}

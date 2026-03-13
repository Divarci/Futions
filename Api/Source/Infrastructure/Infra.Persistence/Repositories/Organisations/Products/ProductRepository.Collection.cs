using Core.Domain.Entities.Organisations.Products;
using Core.Library.ResultPattern;
using Infra.Persistence.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace Infra.Persistence.Repositories.Organisations.Products;

internal sealed partial class ProductRepository
{
    public async Task<Result<Product[]>> GetPaginatedAsync(
        Guid tenantId,
        int page,
        int pageSize,
        string sortBy,
        bool isAscending,
        string? filter,
        CancellationToken cancellationToken = default)
    {
        var query = _context.Set<Product>()
            .AsNoTracking()
            .Where(x => x.TenantId == tenantId && !x.IsDeleted)
            .WhereIf(!string.IsNullOrWhiteSpace(filter), x => EF.Functions.Like(x.Name, $"%{filter}%"))
            .OrderByIf(isAscending, sortBy);

        Product[] products = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToArrayAsync(cancellationToken);

        return Result<Product[]>.Success(
            message: "Products retrieved successfully.",
            data: products,
            statusCode: HttpStatusCode.OK);
    }
}

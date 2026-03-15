using Core.Domain.Entities.Organisations.Products;
using Core.Library.ResultPattern;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace Infra.Persistence.Repositories.Organisations.Products;

internal sealed partial class ProductRepository
{
    public async Task<Result<Product>> GetCompanyProductByIdAsync(
        Guid tenantId,
        Guid companyId,  
        Guid productId,
        CancellationToken cancellationToken = default)
    {
        Product? product = await _context.Set<Product>()
            .AsNoTracking()
            .SingleOrDefaultAsync(x => 
                x.Id == productId &&
                x.TenantId == tenantId && 
                x.CompanyId == companyId && 
                !x.IsDeleted &&
                !x.Company.IsDeleted, cancellationToken);

        if (product is null)        
            return Result<Product>.Failure(
                message: "Product not found.",
                statusCode: HttpStatusCode.NotFound);
        

        return Result<Product>.Success(
            message: "Product retrieved successfully.",
            data: product,
            statusCode: HttpStatusCode.OK);
    }
}



using Core.Domain.Entities.Organisations.Products;
using Core.Domain.Entities.Organisations.Products.Interfaces;
using Infra.Persistence.Context;
using Infra.Persistence.Repositories.Generics;

namespace Infra.Persistence.Repositories.Organisations.Products;

internal sealed partial class ProductRepository(
    AppDbContext context) : TenantedRepository<Product>(context), IProductRepository
{

}


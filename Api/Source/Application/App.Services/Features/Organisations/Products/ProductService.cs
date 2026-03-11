using Core.Domain.Entities.Organisations.Products.Interfaces;

namespace App.Services.Features.Organisations.Companies;

internal sealed partial class ProductService(
    IProductRepository repository) : IProductService
{
    private readonly IProductRepository _repository = repository;
}

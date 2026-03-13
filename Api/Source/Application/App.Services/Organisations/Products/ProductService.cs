using Core.Domain.Entities.Organisations.Companies.Interfaces;
using Core.Domain.Entities.Organisations.Products.Interfaces;
using Core.Library.Contracts.Caching;

namespace App.Services.Features.Organisations.Companies;

internal sealed partial class ProductService(
    IProductRepository repository,
    ICompanyRepository companyRepository,
    ICacheInvalidationService cacheInvalidationService) : IProductService
{
    private readonly IProductRepository _repository = repository;
    private readonly ICompanyRepository _companyRepository = companyRepository;
    private readonly ICacheInvalidationService _cacheInvalidationService = cacheInvalidationService;
}

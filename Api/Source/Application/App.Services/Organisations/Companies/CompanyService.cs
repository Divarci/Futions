using Core.Domain.Entities.Organisations.Companies.Interfaces;
using Core.Domain.Entities.Organisations.CompanyPeople.Interfaces;
using Core.Library.Contracts.Caching;

namespace App.Services.Features.Organisations.Companies;

internal sealed partial class CompanyService(
    ICompanyRepository companyRepository,
    ICompanyPersonRepository companyPersonRepository,
    ICacheInvalidationService cacheInvalidationService) : ICompanyService
{
    private readonly ICompanyRepository _companyRepository = companyRepository;
    private readonly ICompanyPersonRepository _companyPersonRepository = companyPersonRepository;
    private readonly ICacheInvalidationService _cacheInvalidationService = cacheInvalidationService;
}

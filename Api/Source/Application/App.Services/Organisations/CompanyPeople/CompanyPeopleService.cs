using Core.Domain.Entities.Organisations.Companies.Interfaces;
using Core.Domain.Entities.Organisations.CompanyPeople.Interfaces;
using Core.Domain.Entities.Organisations.People.Interfaces;
using Core.Library.Contracts.Caching;

namespace App.Services.Features.Organisations.Companies;

internal sealed partial class CompanyPeopleService(
    ICompanyPersonRepository companyPersonRepository,
    ICompanyRepository companyRepository,
    IPersonRepository personRepository,
    ICacheInvalidationService cacheInvalidationService) : ICompanyPersonService
{
    private readonly ICompanyPersonRepository _companyPersonRepository = companyPersonRepository;
    private readonly ICompanyRepository _companyRepository = companyRepository;
    private readonly IPersonRepository _personRepository = personRepository;
    private readonly ICacheInvalidationService _cacheInvalidationService = cacheInvalidationService;
}

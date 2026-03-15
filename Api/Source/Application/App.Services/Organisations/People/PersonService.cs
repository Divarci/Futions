using Core.Domain.Entities.Organisations.CompanyPeople.Interfaces;
using Core.Domain.Entities.Organisations.People.Interfaces;
using Core.Library.Contracts.Caching;

namespace App.Services.Features.Organisations.People;

internal sealed partial class PersonService(
    IPersonRepository personRepository,
    ICompanyPersonRepository companyPersonRepository,
    ICacheInvalidationService cacheInvalidationService) : IPersonService
{
    private readonly IPersonRepository _personRepository = personRepository;
    private readonly ICompanyPersonRepository _companyPersonRepository = companyPersonRepository;
    private readonly ICacheInvalidationService _cacheInvalidationService = cacheInvalidationService;
}

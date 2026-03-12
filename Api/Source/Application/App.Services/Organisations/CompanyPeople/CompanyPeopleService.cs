using Core.Domain.Entities.Organisations.CompanyPeople.Interfaces;

namespace App.Services.Features.Organisations.Companies;

internal sealed partial class CompanyPeopleService(
    ICompanyPersonRepository companyPersonRepository) : ICompanyPersonService
{
    private readonly ICompanyPersonRepository _companyPersonRepository = companyPersonRepository;
}

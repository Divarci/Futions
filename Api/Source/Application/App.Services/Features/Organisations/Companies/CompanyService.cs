using Core.Domain.Entities.Organisations.Companies.Interfaces;

namespace App.Services.Features.Organisations.Companies;

internal sealed partial class CompanyService(
    ICompanyRepository companyRepository) : ICompanyService
{
    private readonly ICompanyRepository _companyRepository = companyRepository;
}

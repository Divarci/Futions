using Core.Domain.Entities.Organisations.Companies;
using Core.Domain.Entities.Organisations.Companies.Interfaces;
using Infra.Persistence.Context;
using Infra.Persistence.Repositories.Generics;

namespace Infra.Persistence.Repositories.Organisations.Companies;

internal sealed partial class CompanyRepository(
    AppDbContext context) : TenantedRepository<Company>(context), ICompanyRepository
{

}

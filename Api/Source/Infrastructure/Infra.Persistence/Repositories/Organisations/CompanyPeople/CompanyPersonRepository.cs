using Core.Domain.Entities.Organisations.CompanyPeople;
using Core.Domain.Entities.Organisations.CompanyPeople.Interfaces;
using Infra.Persistence.Context;
using Infra.Persistence.Repositories.Generics;

namespace Infra.Persistence.Repositories.Organisations.CompanyPeople;

internal sealed partial class CompanyPersonRepository(
    AppDbContext context) : GlobalRepository<CompanyPerson>(context), ICompanyPersonRepository
{
}

using Core.Domain.Entities.Organisations.People;
using Core.Domain.Entities.Organisations.People.Interfaces;
using Infra.Persistence.Context;
using Infra.Persistence.Repositories.Generics;

namespace Infra.Persistence.Repositories.Organisations.People;

internal sealed partial class PersonRepository(
    AppDbContext context) : TenantedRepository<Person>(context), IPersonRepository
{

}

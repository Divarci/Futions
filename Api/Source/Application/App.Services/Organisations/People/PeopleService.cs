using Core.Domain.Entities.Organisations.People.Interfaces;

namespace App.Services.Features.Organisations.Companies;

internal sealed partial class PeopleService(
    IPersonRepository personRepository) : IPersonService
{
    private readonly IPersonRepository _personRepository = personRepository;
}

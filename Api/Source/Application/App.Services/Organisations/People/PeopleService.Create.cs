using Core.Domain.Entities.Organisations.People;
using Core.Domain.Entities.Organisations.People.Models;
using Core.Library.ResultPattern;

namespace App.Services.Features.Organisations.Companies;

internal sealed partial class PeopleService
{
    public Task<Result<Person>> CreateAsync(Guid tenantId, PersonCreateModel createModel, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
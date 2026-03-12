using Core.Domain.Entities.Organisations.People;
using Core.Library.ResultPattern;

namespace App.Services.Features.Organisations.Companies;

internal sealed partial class PeopleService
{
    public Task<Result<Person>> GetByIdAsync(Guid tenantId, Guid id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
using Core.Domain.Entities.Organisations.People;
using Core.Library.ResultPattern;

namespace App.Services.Features.Organisations.Companies;

internal sealed partial class PeopleService
{
    public async Task<Result<Person>> GetByIdAsync(
        Guid tenantId,
        Guid id,
        CancellationToken cancellationToken = default)
    {
        // Get the person by id and tenant id
        return await _personRepository
            .GetByIdAsync(id, tenantId, cancellationToken);
    }
}
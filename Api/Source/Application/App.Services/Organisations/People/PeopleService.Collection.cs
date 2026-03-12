using Core.Domain.Entities.Organisations.People;
using Core.Library.ResultPattern;

namespace App.Services.Features.Organisations.Companies;

internal sealed partial class PeopleService
{
    public Task<Result<Person[]>> GetPaginatedAsync(Guid tenantId, int page, int pageSize, string sortBy, bool isAscending, string? filter, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
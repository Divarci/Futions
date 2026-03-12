using Core.Domain.Entities.Organisations.People;
using Core.Library.ResultPattern;

namespace App.Services.Features.Organisations.Companies;

internal sealed partial class PeopleService
{
    public Task<Result<Person[]>> GetPaginatedAsync(Guid tenantId, int? pageQuery, int? pageSizeQuery, string? sortByQuery, bool? isAscendingQuery, string? filterQuery, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
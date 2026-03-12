using Core.Domain.Entities.Organisations.CompanyPeople;
using Core.Library.ResultPattern;

namespace App.Services.Features.Organisations.Companies;

internal sealed partial class CompanyPeopleService
{
    public Task<Result<CompanyPerson[]>> GetPaginatedAsync(Guid tenantId, int? pageQuery, int? pageSizeQuery, string? sortByQuery, bool? isAscendingQuery, string? filterQuery, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
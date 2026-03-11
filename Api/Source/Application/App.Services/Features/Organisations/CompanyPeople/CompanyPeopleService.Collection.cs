using Core.Domain.Entities.Organisations.CompanyPeople;
using Core.Library.ResultPattern;

namespace App.Services.Features.Organisations.Companies;

internal sealed partial class CompanyPeopleService
{
    public Task<Result<CompanyPerson[]>> GetPaginatedAsync(Guid tenantId, int page, int pageSize, string sortBy, bool isAscending, string? filter, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
using Core.Domain.Entities.Organisations.Companies;
using Core.Library.ResultPattern;

namespace App.Services.Features.Organisations.Companies;

internal sealed partial class CompanyService
{
    public Task<Result<Company[]>> GetPaginatedAsync(Guid tenantId, int? pageQuery, int? pageSizeQuery, string? sortByQuery, bool? isAscendingQuery, string? filterQuery, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
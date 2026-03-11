using Core.Domain.Entities.Organisations.Companies;
using Core.Library.ResultPattern;

namespace App.Services.Features.Organisations.Companies;

internal sealed partial class CompanyService
{
    public Task<Result<Company[]>> GetPaginatedAsync(Guid tenantId, int page, int pageSize, string sortBy, bool isAscending, string? filter, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
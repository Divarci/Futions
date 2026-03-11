using Core.Domain.Entities.Organisations.Companies;
using Core.Library.ResultPattern;

namespace App.Services.Features.Organisations.Companies;

internal sealed partial class CompanyService
{
    public Task<Result<Company>> GetByIdAsync(Guid tenantId, Guid id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
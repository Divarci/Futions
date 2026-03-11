using Core.Domain.Entities.Organisations.CompanyPeople;
using Core.Library.ResultPattern;

namespace App.Services.Features.Organisations.Companies;

internal sealed partial class CompanyPeopleService
{
    public Task<Result<CompanyPerson>> GetByIdAsync(Guid tenantId, Guid id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
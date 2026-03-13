using Core.Domain.Entities.Organisations.CompanyPeople;
using Core.Library.ResultPattern;

namespace App.Services.Features.Organisations.Companies;

internal sealed partial class CompanyPeopleService
{
    public async Task<Result<CompanyPerson>> GetByIdAsync(
        Guid tenantId,
        Guid id,
        CancellationToken cancellationToken = default)
    {
        // Get the company person by id for tenant.
        return await _companyPersonRepository
            .GetByIdAsync(tenantId,id, cancellationToken);
    }
}
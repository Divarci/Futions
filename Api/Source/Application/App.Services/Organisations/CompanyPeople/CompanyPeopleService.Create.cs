using Core.Domain.Entities.Organisations.CompanyPeople;
using Core.Domain.Entities.Organisations.CompanyPeople.Models;
using Core.Library.ResultPattern;

namespace App.Services.Features.Organisations.Companies;

internal sealed partial class CompanyPeopleService
{
    public Task<Result<CompanyPerson>> CreateAsync(Guid tenantId, CompanyPersonCreateModel createModel, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
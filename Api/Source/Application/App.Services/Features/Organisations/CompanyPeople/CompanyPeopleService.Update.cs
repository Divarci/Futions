using Core.Domain.Entities.Organisations.CompanyPeople.Models;
using Core.Library.ResultPattern;

namespace App.Services.Features.Organisations.Companies;

internal sealed partial class CompanyPeopleService
{
    public Task<Result> UpdateAsync(Guid tenantId, CompanyPersonUpdateModel updateModel, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
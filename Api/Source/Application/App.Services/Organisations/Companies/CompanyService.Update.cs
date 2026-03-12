using Core.Domain.Entities.Organisations.Companies.Models;
using Core.Library.ResultPattern;

namespace App.Services.Features.Organisations.Companies;

internal sealed partial class CompanyService
{
    public Task<Result> UpdateAsync(Guid tenantId, CompanyUpdateModel updateModel, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
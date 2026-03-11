using Core.Domain.Entities.Organisations.Companies;
using Core.Domain.Entities.Organisations.Companies.Models;
using Core.Library.ResultPattern;

namespace App.Services.Features.Organisations.Companies;

internal sealed partial class CompanyService
{
    public Task<Result<Company>> CreateAsync(Guid tenantId, CompanyCreateModel createModel, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
using Core.Domain.Entities.Organisations.Companies;
using Core.Library.ResultPattern;

namespace App.Services.Features.Organisations.Companies;

internal sealed partial class CompanyService
{
    public async Task<Result<Company>> GetByIdAsync(
        Guid tenantId, 
        Guid id, 
        CancellationToken cancellationToken = default)
    {
        // Get the company by id and tenant id
        return await _companyRepository
            .GetByIdAsync(id, tenantId, cancellationToken);        
    }
}
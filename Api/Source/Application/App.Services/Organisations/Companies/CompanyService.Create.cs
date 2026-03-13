using Core.Domain.Entities.Organisations.Companies;
using Core.Domain.Entities.Organisations.Companies.Models;
using Core.Library.ResultPattern;

namespace App.Services.Features.Organisations.Companies;

internal sealed partial class CompanyService
{
    public async Task<Result<Company>> CreateAsync(
        Guid tenantId, 
        CompanyCreateModel createModel, 
        CancellationToken cancellationToken = default)
    {
        // Get Tenant and ensure tenantId is valid and exists.Since this is an example,
        // we will skip this step and assume tenantId is valid and exists.

        // Create Company entity from the create model and tenantId.
        Result<Company> companyCreateResult = Company.Create(createModel, tenantId);
        
        if (companyCreateResult.IsFailureAndNoData)
            return companyCreateResult;

        // Persist the new Company entity to the database.
        await _companyRepository.CreateAsync(companyCreateResult.Data!, cancellationToken);

        // Create cache key
        string cacheKey = $"{nameof(Company)}:tenant({tenantId}):id({companyCreateResult.Data.Id})";

        // Invalidate the cache for the newly created company and the collections that may include it.
        await _cacheInvalidationService.InvalidateEntity(cacheKey);
        await _cacheInvalidationService.InvalidateCollections();

        return companyCreateResult;
    }
}
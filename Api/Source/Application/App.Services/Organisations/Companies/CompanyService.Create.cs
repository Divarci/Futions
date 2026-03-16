using Core.Domain.Entities.Organisations.Companies;
using Core.Domain.Entities.Organisations.Companies.Models;
using Core.Library.ResultPattern;

namespace App.Services.Features.Organisations.Companies;

internal sealed partial class CompanyService
{
    public async Task<Result<Company>> CreateCompanyAsync(
        CompanyCreateModel createModel,
        Func<string, (string Label, object Value)[], string> cacheKeyBuilder,
        CancellationToken cancellationToken = default)
    {
        // Create Company entity from the create model.
        Result<Company> companyCreateResult = Company.Create(createModel);

        if (companyCreateResult.IsFailureAndNoData)
            return companyCreateResult;

        // Persist the new Company entity to the database.
        await _companyRepository.CreateAsync(companyCreateResult.Data!, cancellationToken);

        // Invalidate the cache for the newly created company and the collections that may include it.
        string cacheKey = cacheKeyBuilder(nameof(Company), [("tenant", createModel.TenantId), ("company", companyCreateResult.Data.Id)]);
        await _cacheInvalidationService.InvalidateEntity(cacheKey);
        await _cacheInvalidationService.InvalidateCollections();

        return companyCreateResult;
    }
}
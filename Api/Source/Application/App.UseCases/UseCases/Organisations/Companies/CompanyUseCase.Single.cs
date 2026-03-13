using Core.Domain.Entities.Organisations.Companies;
using Core.Library.ResultPattern;

namespace App.UseCases.UseCases.Organisations.Companies;

internal sealed partial class CompanyUseCase
{   
    public async Task<Result<Company>> GetByIdAsync(
        Guid tenantId,
        Guid id,
        CancellationToken cancellationToken = default)
    {
        // Create a unique cache key for the company based on tenant ID and company ID
        string cacheKey = $"{nameof(Company)}:tenant({tenantId}):id({id})";

        // Attempt to retrieve the company from the cache. If it's not present, fetch it from the service and cache the result.
        Result<Company> companyResult = await _cacheProvider.GetSingleAsync(
            serviceMethod: async () => await _companyService.GetByIdAsync(
                tenantId, id, cancellationToken),
            useCache: true,
            cacheKey: cacheKey,
            new(1,0,0));

        return companyResult;
    }
}

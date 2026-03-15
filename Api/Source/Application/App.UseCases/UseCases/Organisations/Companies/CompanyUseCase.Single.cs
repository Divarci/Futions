using Core.Domain.Entities.Organisations.Companies;
using Core.Library.ResultPattern;

namespace App.UseCases.UseCases.Organisations.Companies;

internal sealed partial class CompanyUseCase
{   
    public async Task<Result<TDto>> GetByIdAsync<TDto>(
        Guid tenantId,
        Guid id,
        Func<Company, TDto> mapper,
        CancellationToken cancellationToken = default) where TDto : class
    {
        // Create a unique cache key for the company based on tenant ID and company ID
        string cacheKey = $"{nameof(Company)}:tenant({tenantId}):id({id})";

        // Attempt to retrieve the company from the cache. If it's not present, fetch it from the service and cache the result.
        Result<TDto> companyResult = await _cacheProvider.GetSingleAsync(
            serviceMethod: async () => await _companyService.GetByIdAsync(
                tenantId, id, mapper, cancellationToken),
            useCache: true,
            cacheKey: cacheKey,
            _timeout);

        return companyResult;
    }
}

using Core.Domain.Entities.Organisations.CompanyPeople;
using Core.Library.ResultPattern;

namespace App.UseCases.UseCases.Organisations.CompanyPeople;

internal sealed partial class CompanyPersonUseCase
{
    public async Task<Result<TDto>> GetCompanyPersonByIdAsync<TDto>(
        Guid tenantId,
        Guid companyId,
        Guid companyPersonId,
        Func<CompanyPerson, TDto> mapper,
        CancellationToken cancellationToken = default) where TDto : class
    {
        // Create a unique cache key for the company person based on tenant ID and company person ID.
        string cacheKey = $"{nameof(CompanyPerson)}:tenant({tenantId}):company({companyId}):companyPerson({companyPersonId})";

        // Attempt to retrieve the company person from the cache. If it's not present, fetch it from the service and cache the result.
        Result<TDto> companyPersonResult = await _cacheProvider.GetSingleAsync(
            serviceMethod: async () => await _companyPersonService.GetCompanyPersonByIdAsync(
                tenantId, companyId, companyPersonId, mapper, cancellationToken),
            useCache: true,
            cacheKey: cacheKey,
            _timeout);

        return companyPersonResult;
    }
}

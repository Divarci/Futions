using App.UseCases.Helpers;
using Core.Domain.Entities.Organisations.People;
using Core.Library.ResultPattern;

namespace App.UseCases.UseCases.Organisations.People;

internal sealed partial class PersonUseCase
{
    public async Task<Result<TDto>> GetPersonByIdAsync<TDto>(
        Guid tenantId,
        Guid personId,
        Func<Person, TDto> mapper,
        CancellationToken cancellationToken = default) where TDto : class
    {
        // Create a unique cache key for the person based on tenant ID and person ID.
        string cacheKey = CacheKeyHelper.Single(nameof(Person), ("tenant", tenantId), ("person", personId));

        // Attempt to retrieve the person from the cache. If it's not present, fetch it from the service and cache the result.
        Result<TDto> personResult = await _cacheProvider.GetSingleAsync(
            serviceMethod: async () => await _personService.GetPersonByIdAsync(
                tenantId, personId, mapper, cancellationToken),
            useCache: true,
            cacheKey: cacheKey,
            _timeout);

        return personResult;
    }
}

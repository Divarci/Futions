using Core.Domain.Entities.Organisations.People;
using Core.Library.ResultPattern;

namespace App.UseCases.UseCases.Organisations.People;

internal sealed partial class PersonUseCase
{
    public async Task<Result<Person>> GetByIdAsync(
        Guid tenantId,
        Guid id,
        CancellationToken cancellationToken = default)
    {
        // Create a unique cache key for the person based on tenant ID and person ID.
        string cacheKey = $"{nameof(Person)}:tenant({tenantId}):id({id})";

        // Attempt to retrieve the person from the cache. If it's not present, fetch it from the service and cache the result.
        Result<Person> personResult = await _cacheProvider.GetSingleAsync(
            serviceMethod: async () => await _personService.GetByIdAsync(
                tenantId, id, cancellationToken),
            useCache: true,
            cacheKey: cacheKey,
            _timeout);

        return personResult;
    }
}

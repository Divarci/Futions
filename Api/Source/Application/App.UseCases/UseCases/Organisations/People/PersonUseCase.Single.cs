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
        string cacheKey = $"{nameof(Person)}:tenant({tenantId}):id({id})";

        Result<Person> personResult = await _cacheProvider.GetSingleAsync(
            serviceMethod: async () => await _personService.GetByIdAsync(
                tenantId, id, cancellationToken),
            useCache: true,
            cacheKey: cacheKey,
            new(1, 0, 0));

        return personResult;
    }
}

using Core.Domain.Entities.Organisations.People;
using Core.Domain.Entities.Organisations.People.Models;
using Core.Library.ResultPattern;

namespace App.Services.Features.Organisations.People;

internal sealed partial class PersonService
{
    public async Task<Result<Person>> CreatePersonAsync(
        PersonCreateModel createModel,
        Func<string, (string Label, object Value)[], string> cacheKeyBuilder,
        CancellationToken cancellationToken = default)
    {
        // Create Person entity from the create model.
        Result<Person> personCreateResult = Person.Create(createModel);

        if (personCreateResult.IsFailureAndNoData)
            return personCreateResult;

        // Persist the new Person entity to the database.
        await _personRepository.CreateAsync(personCreateResult.Data!, cancellationToken);

        // Invalidate the cache for the newly created person and the collections that may include it.
        string cacheKey = cacheKeyBuilder(nameof(Person), [("tenant", createModel.TenantId), ("person", personCreateResult.Data.Id)]);
        await _cacheInvalidationService.InvalidateEntity(cacheKey);
        await _cacheInvalidationService.InvalidateCollections();

        return personCreateResult;
    }
}
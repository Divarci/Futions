using Core.Domain.Entities.Organisations.People;
using Core.Domain.Entities.Organisations.People.Models;
using Core.Library.ResultPattern;

namespace App.Services.Features.Organisations.Companies;

internal sealed partial class PersonService
{
    public async Task<Result<Person>> CreateAsync(
        PersonCreateModel createModel,
        CancellationToken cancellationToken = default)
    {
        // Create Person entity from the create model.
        Result<Person> personCreateResult = Person.Create(createModel);

        if (personCreateResult.IsFailureAndNoData)
            return personCreateResult;

        // Persist the new Person entity to the database.
        await _personRepository.CreateAsync(personCreateResult.Data!, cancellationToken);

        // Create cache key
        string cacheKey = $"{nameof(Person)}:tenant({createModel.TenantId}):id({personCreateResult.Data!.Id})";

        // Invalidate the cache for the newly created person and the collections that may include it.
        await _cacheInvalidationService.InvalidateEntity(cacheKey);
        await _cacheInvalidationService.InvalidateCollections();

        return personCreateResult;
    }
}
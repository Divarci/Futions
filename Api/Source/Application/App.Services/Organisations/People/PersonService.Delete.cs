using Core.Domain.Entities.Organisations.People;
using Core.Library.ResultPattern;
using System.Net;

namespace App.Services.Features.Organisations.People;

internal sealed partial class PersonService
{
    public async Task<Result> DeletePersonAsync(
        Guid tenantId,
        Guid personId,
        Func<string, (string Label, object Value)[], string> cacheKeyBuilder,
        CancellationToken cancellationToken = default)
    {
        // Check if the person exists
        Result<Person> personResult = await _personRepository
            .GetByIdAsync(personId, tenantId, cancellationToken);

        if (personResult.IsFailureAndNoData)
            return personResult;

        // Check if the person has associated companies
        Result<bool> hasCompaniesResult = await _companyPersonRepository
            .HasCompanyAsync(tenantId, personId, cancellationToken);

        if (hasCompaniesResult.IsFailureAndNoData)
            return hasCompaniesResult;

        _personRepository.Delete(personResult.Data!);

        // Invalidate the cache for the deleted person and the collections that may include it.
        string cacheKey = cacheKeyBuilder(nameof(Person), [("tenant", tenantId), ("person", personId)]);
        await _cacheInvalidationService.InvalidateEntity(cacheKey);
        await _cacheInvalidationService.InvalidateCollections();

        return Result.Success(
            message: "Person deleted successfully.",
            statusCode: HttpStatusCode.OK);
    }
}
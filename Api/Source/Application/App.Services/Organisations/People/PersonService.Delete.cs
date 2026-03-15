using Core.Domain.Entities.Organisations.People;
using Core.Library.ResultPattern;
using System.Net;

namespace App.Services.Features.Organisations.People;

internal sealed partial class PersonService
{
    public async Task<Result> DeletePersonAsync(
        Guid tenantId,
        Guid personId,
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

        // Create cache key
        string cacheKey = $"{nameof(Person)}:tenant({tenantId}):person({personId})";

        // Invalidate the cache for the deleted person and the collections that may include it.
        await _cacheInvalidationService.InvalidateEntity(cacheKey);
        await _cacheInvalidationService.InvalidateCollections();

        return Result.Success(
            message: "Person deleted successfully.",
            statusCode: HttpStatusCode.OK);
    }
}
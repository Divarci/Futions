using Core.Domain.Entities.Organisations.CompanyPeople;
using Core.Domain.Entities.Organisations.CompanyPeople.Models;
using Core.Library.ResultPattern;
using System.Net;

namespace App.Services.Features.Organisations.Companies;

internal sealed partial class CompanyPersonService
{
    public async Task<Result<CompanyPerson>> CreateCompanyPersonAsync(
        Guid tenantId,
        CompanyPersonCreateModel createModel,
        Func<string, (string Label, object Value)[], string> cacheKeyBuilder,
        CancellationToken cancellationToken = default)
    {
        // Get the Company to ensure it exists before creating the CompanyPerson entity.
        Result<bool> companyExistsResult = await _companyRepository
            .ExistsAsync(createModel.CompanyId, tenantId, cancellationToken);

        if (companyExistsResult.IsFailure)
            return Result<CompanyPerson>.Failure(
                message: "Company does not exist.",
                statusCode: HttpStatusCode.NotFound);

        // Get the Person to ensure it exists before creating the CompanyPerson entity.
        Result<bool> personExistsResult = await _personRepository
            .ExistsAsync(createModel.PersonId, tenantId, cancellationToken);

        if (personExistsResult.IsFailure)
            return Result<CompanyPerson>.Failure(
                message: "Person does not exist.",
                statusCode: HttpStatusCode.NotFound);

        // Check if a CompanyPerson entity already exists for the given CompanyId and PersonId.
        Result<bool> companyPersonExistsResult = await _companyPersonRepository
            .HasCompanyPersonAsync(createModel.CompanyId, createModel.PersonId, cancellationToken);

        if(companyPersonExistsResult.IsSuccess)
            return Result<CompanyPerson>.Failure(
                message: "Company person already exists for the given Company and Person.",
                statusCode: HttpStatusCode.BadRequest);

        // Create CompanyPerson entity from the create model.
        Result<CompanyPerson> companyPersonCreateResult = CompanyPerson.Create(createModel);

        if (companyPersonCreateResult.IsFailureAndNoData)
            return companyPersonCreateResult;

        // Persist the new CompanyPerson entity to the database.
        Result<CompanyPerson> persistResult = await _companyPersonRepository.CreateAsync(companyPersonCreateResult.Data!, cancellationToken);

        if (persistResult.IsFailureAndNoData)
            return persistResult;

        // Invalidate the cache for the newly created company person and the collections that may include it.
        string cacheKey = cacheKeyBuilder(nameof(CompanyPerson), [("tenant", tenantId), ("company", createModel.CompanyId), ("companyPerson", companyPersonCreateResult.Data!.Id)]);
        await _cacheInvalidationService.InvalidateEntity(cacheKey);
        await _cacheInvalidationService.InvalidateCollections();

        return companyPersonCreateResult;
    }
}
using Core.Domain.Entities.Organisations.Companies;
using Core.Library.ResultPattern;
using System.Net;

namespace App.Services.Features.Organisations.Companies;

internal sealed partial class CompanyService
{
    public async Task<Result> DeleteCompanyAsync(
        Guid tenantId,
        Guid companyId,
        Func<string, (string Label, object Value)[], string> cacheKeyBuilder,
        CancellationToken cancellationToken = default)
    {
        // Check if the company exists
        Result<Company> companyResult = await _companyRepository
            .GetByIdAsync(companyId, tenantId, cancellationToken);

        if(companyResult.IsFailureAndNoData)
            return companyResult;

        // Check if the company has associated people
        Result<bool> hasCompanyPeopleResult = await _companyPersonRepository
            .HasPeopleAsync(tenantId, companyId, cancellationToken);
            
        if (hasCompanyPeopleResult.IsFailureAndNoData)
            return hasCompanyPeopleResult;

        if (hasCompanyPeopleResult.Data!)
            return Result.Failure(
                message: "Cannot delete company with associated people. Please remove associated people first.",
                statusCode: HttpStatusCode.BadRequest);

        // Soft delete the company
        _companyRepository.Delete(companyResult.Data);

        // Invalidate the cache for the newly created company and the collections that may include it.
        string cacheKey = cacheKeyBuilder(nameof(Company), [("tenant", tenantId), ("company", companyId)]);
        await _cacheInvalidationService.InvalidateEntity(cacheKey);
        await _cacheInvalidationService.InvalidateCollections();

        return Result.Success(
            message: "Company deleted successfully.",
            statusCode: HttpStatusCode.OK);
    }
}
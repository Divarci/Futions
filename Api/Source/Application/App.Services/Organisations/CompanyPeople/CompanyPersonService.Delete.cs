using Core.Domain.Entities.Organisations.CompanyPeople;
using Core.Library.ResultPattern;
using System.Net;

namespace App.Services.Features.Organisations.Companies;

internal sealed partial class CompanyPersonService
{
    public async Task<Result> DeleteCompanyPersonAsync(
        Guid tenantId,
        Guid companyId,
        Guid companyPersonId,
        CancellationToken cancellationToken = default)
    {
        // Check if the company person belongs to the tenant.
        Result<bool> tenantCheckResult = await _companyPersonRepository
            .CheckIfBelongsToTenantAsync(tenantId, companyId, companyPersonId, cancellationToken);

        if (tenantCheckResult.IsFailureAndNoData)
            return tenantCheckResult;

        // Check if the company person exists.
        Result<CompanyPerson> companyPersonResult = await _companyPersonRepository
            .GetCompanyPersonByIdAsync(tenantId, companyId, companyPersonId, cancellationToken);

        if (companyPersonResult.IsFailureAndNoData)
            return companyPersonResult;

        // Hard delete the company person record from the database.
        _companyPersonRepository.Delete(companyPersonResult.Data!);

        return Result.Success(
            message: "Company person deleted successfully.",
            statusCode: HttpStatusCode.OK);
    }
}
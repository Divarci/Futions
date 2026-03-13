using Core.Domain.Entities.Organisations.Companies;
using Core.Domain.Entities.Organisations.CompanyPeople;
using Core.Library.ResultPattern;
using System.Net;

namespace App.Services.Features.Organisations.Companies;

internal sealed partial class CompanyService
{
    public async Task<Result> DeleteAsync(
        Guid tenantId,
        Guid id,
        CancellationToken cancellationToken = default)
    {
        // Check if the company exists
        Result<Company> companyResult = await _companyRepository
            .GetByIdAsync(id, tenantId, cancellationToken);

        if(companyResult.IsFailureAndNoData)
            return companyResult;

        // Check if the company has associated people
        Result<bool> hasCompanyPeopleResult = await _companyPersonRepository
            .HasCompanyPeopleAsync(id, tenantId, cancellationToken);
            
        if (hasCompanyPeopleResult.IsFailureAndNoData)
            return hasCompanyPeopleResult;

        if (hasCompanyPeopleResult.Data!)
            return Result.Failure(
                message: "Cannot delete company with associated people. Please remove associated people first.",
                statusCode: HttpStatusCode.BadRequest);

        // Soft delete the company
        _companyRepository.Delete(companyResult.Data!);
                
        return Result.Success(
            message: "Company deleted successfully.",
            statusCode: HttpStatusCode.OK);
    }
}
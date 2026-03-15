using Core.Domain.Entities.Organisations.CompanyPeople;
using Core.Domain.Entities.Organisations.CompanyPeople.Models;
using Core.Library.ResultPattern;
using System.Net;

namespace App.Services.Features.Organisations.Companies;

internal sealed partial class CompanyPersonService
{
    public async Task<Result> UpdateCompanyPersonAsync(
        CompanyPersonUpdateModel updateModel,
        CancellationToken cancellationToken = default)
    {
        // Retrieve the company person to update.
        Result<CompanyPerson> companyPersonResult = await _companyPersonRepository
            .GetCompanyPersonByIdAsync(updateModel.TenantId, updateModel.CompanyId, updateModel.CompanyPersonId, cancellationToken);

        if (companyPersonResult.IsFailureAndNoData)
            return companyPersonResult;

        CompanyPerson companyPerson = companyPersonResult.Data!;

        // If title is provided, update it; otherwise, keep the existing title.
        if (!string.IsNullOrWhiteSpace(updateModel.Title))
        {
            Result updateTitleResult = companyPerson
                .UpdateTitle(updateModel.Title);

            if (updateTitleResult.IsFailure)
                return updateTitleResult;
        }

        // Persist the updated company person entity.
        _companyPersonRepository.Update(companyPerson);

        return Result.Success(
            message: "Company person updated successfully.",
            statusCode: HttpStatusCode.OK);
    }
}
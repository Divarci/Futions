using Core.Domain.Entities.Organisations.Companies;
using Core.Domain.Entities.Organisations.Companies.Models;
using Core.Library.ResultPattern;
using System.Net;

namespace App.Services.Features.Organisations.Companies;

internal sealed partial class CompanyService
{
    public async Task<Result> UpdateCompanyAsync(
        CompanyUpdateModel updateModel,
        Func<string, (string Label, object Value)[], string> cacheKeyBuilder,
        CancellationToken cancellationToken = default)
    {
        // Retrieve the company to update
        Result<Company> companyResult = await _companyRepository
            .GetByIdAsync(updateModel.CompanyId, updateModel.TenantId, cancellationToken);

        if (companyResult.IsFailureAndNoData)
            return companyResult;

        Company company = companyResult.Data;

        // Update the company's properties based on the provided update model
        // If name is provided, update it; otherwise, keep the existing name
        if (!string.IsNullOrWhiteSpace(updateModel.Name))
        {
            Result updateNameResult = company
                .UpdateName(updateModel.Name);

            if (updateNameResult.IsFailure)
                return updateNameResult;
        }

        // If address model is provided, update the company's address; otherwise, keep the existing address
        if (updateModel.AddressModel is not null)
        {
            Result updateAddressResult = company
                .UpdateAddress(updateModel.AddressModel);

            if (updateAddressResult.IsFailure)
                return updateAddressResult;
        }

        // Persist the updated company entity
        _companyRepository.Update(company);

        // Invalidate the cache for the newly created company and the collections that may include it.
        string cacheKey = cacheKeyBuilder(nameof(Company), [("tenant", updateModel.TenantId), ("company", updateModel.CompanyId)]);
        await _cacheInvalidationService.InvalidateEntity(cacheKey);
        await _cacheInvalidationService.InvalidateCollections();

        return Result.Success(
            message: "Company updated successfully.",
            statusCode: HttpStatusCode.OK);
    }
}
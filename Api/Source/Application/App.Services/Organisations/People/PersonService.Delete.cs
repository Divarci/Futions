using Core.Domain.Entities.Organisations.People;
using Core.Library.ResultPattern;
using System.Net;

namespace App.Services.Features.Organisations.Companies;

internal sealed partial class PersonService
{
    public async Task<Result> DeleteAsync(
        Guid tenantId,
        Guid id,
        CancellationToken cancellationToken = default)
    {
        // Check if the person exists
        Result<Person> personResult = await _personRepository
            .GetByIdAsync(id, tenantId, cancellationToken);

        if (personResult.IsFailureAndNoData)
            return personResult;

        // Check if the person has associated companies
        Result<bool> hasCompaniesResult = await _companyPersonRepository
            .HasCompanyAsync(tenantId, id, cancellationToken);

        if (hasCompaniesResult.IsFailure)
            return hasCompaniesResult;

        _personRepository.Delete(personResult.Data!);

        return Result.Success(
            message: "Person deleted successfully.",
            statusCode: HttpStatusCode.OK);
    }
}
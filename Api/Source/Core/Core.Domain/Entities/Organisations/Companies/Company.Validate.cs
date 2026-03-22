using Core.Library.ResultPattern;
using Core.Library.Validators;
using System.Net;

namespace Core.Domain.Entities.Organisations.Companies;

public partial class Company
{
    private static Result Validate(Company company)
    {
        List<Result> results = [];

        ValidateProperties(results, company);
        ValidateBusiness(results, company);

        return Result.CombineValidationErrors(results);
    }

    private static void ValidateProperties(List<Result> results, Company company)
        => results.AddRange(
            [
                company.Name.Validate(
                    fieldName: nameof(company.Name),
                    maxLength: 1),
                company.TenantId.Validate(
                    fieldName: nameof(company.TenantId),
                    allowEmpty: false)
            ]);

    private static void ValidateBusiness(List<Result> results, Company company)
    {
        if (company.Name.StartsWith("X"))
            results.Add(Result.Failure(
                message: "Validation failed",
                errorDetails: ErrorDetails.Create(["Company name cannot start with X."]),
                statusCode: HttpStatusCode.UnprocessableContent));
    }

}

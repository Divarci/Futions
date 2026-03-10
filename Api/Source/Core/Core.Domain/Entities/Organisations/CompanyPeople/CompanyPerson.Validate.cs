using Core.Library.ResultPattern;
using Core.Library.Validators;
using System.Net;

namespace Core.Domain.Entities.Organisations.CompanyPeople;

public sealed partial class CompanyPerson
{
    private static Result Validate(CompanyPerson companyPerson)
    {
        List<Result> results = [];

        ValidateProperties(results, companyPerson);
        ValidateBusiness(results, companyPerson);

        return Result.CombineValidationErrors(results);
    }

    private static void ValidateProperties(List<Result> results, CompanyPerson companyPerson)
        => results.AddRange(
            [
                companyPerson.Title.Validate(
                    fieldName: nameof(companyPerson.Title),
                    maxLength: 100),
                companyPerson.CompanyId.Validate(
                    fieldName: nameof(companyPerson.CompanyId),
                    allowEmpty: false),
                companyPerson.PersonId.Validate(
                    fieldName: nameof(companyPerson.PersonId),
                    allowEmpty: false)
            ]);

    private static void ValidateBusiness(List<Result> results, CompanyPerson companyPerson)
    {
        if (companyPerson.Title.StartsWith("X"))
            results.Add(Result.Failure(
                message: "Validation failed",
                errorDetails: ErrorDetails.Create(["Company person title cannot start with X."]),
                statusCode: HttpStatusCode.UnprocessableContent));
    }
}

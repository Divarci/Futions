using Core.Library.ResultPattern;
using Core.Library.Validators;
using System.Net;

namespace Core.Domain.ValueObjects.FullnameValueObject;

public sealed partial record Fullname
{
    private static Result Validate(Fullname fullname)
    {
        List<Result> results = [];

        ValidateProperties(results, fullname);
        ValidateBusiness(results, fullname);

        return Result.CombineValidationErrors(results);
    }

    private static void ValidateProperties(List<Result> results, Fullname fullname)
        => results.AddRange(
            [
                fullname.FirstName.Validate(
                    fieldName: nameof(fullname.FirstName),
                    maxLength: 50),
                fullname.LastName.Validate(
                    fieldName: nameof(fullname.LastName),
                    maxLength: 50),
            ]);

    private static void ValidateBusiness(List<Result> results, Fullname fullname)
    {
        if (!string.IsNullOrEmpty(fullname.FirstName) && char.IsDigit(fullname.FirstName[0]))
            results.Add(Result.Failure(
                message: "Validation failed",
                errorDetails: ErrorDetails.Create(["First name cannot start with a digit."]),
                statusCode: HttpStatusCode.UnprocessableContent));

        if (!string.IsNullOrEmpty(fullname.LastName) && char.IsDigit(fullname.LastName[0]))
            results.Add(Result.Failure(
                message: "Validation failed",
                errorDetails: ErrorDetails.Create(["Last name cannot start with a digit."]),
                statusCode: HttpStatusCode.UnprocessableContent));
    }
}

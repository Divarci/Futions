using Core.Library.ResultPattern;
using Core.Library.Validators;
using System.Net;

namespace Core.Domain.Entities.Organisations.People;

public partial class Person
{
    private static Result Validate(Person person)
    {
        List<Result> results = [];

        ValidateProperties(results, person);
        ValidateBusiness(results, person);

        return Result.CombineValidationErrors(results);
    }

    private static void ValidateProperties(List<Result> results, Person person)
        => results.AddRange(
            [
                person.Email.Validate(
                    fieldName: nameof(person.Email),
                    maxLength: 100,
                    isEmail: true),
                person.TenantId.Validate(
                    fieldName: nameof(person.TenantId),
                    allowEmpty: false)
            ]);

    private static void ValidateBusiness(List<Result> results, Person person)
    {
        if (person.Email.StartsWith("X"))
            results.Add(Result.Failure(
                message: "Validation failed",
                errorDetails: ErrorDetails.Create(["Person email cannot start with X."]),
                statusCode: HttpStatusCode.UnprocessableContent));
    }
}

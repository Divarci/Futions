using Core.Library.ResultPattern;
using Core.Library.Validators;
using System.Net;

namespace Core.Domain.ValueObjects.AddressValueObject;

public sealed partial record Address
{
    private static Result Validate(Address address)
    {
        List<Result> results = [];

        ValidateProperties(results, address);
        ValidateBusiness(results, address);

        return Result.CombineValidationErrors(results);
    }

    private static void ValidateProperties(List<Result> results, Address address)
        => results.AddRange(
            [
                address.LineOne.Validate(
                    fieldName: nameof(address.LineOne),
                    maxLength: 100),
                address.LineTwo.Validate(
                    fieldName: nameof(address.LineTwo),
                    maxLength: 100),
                address.LineThree.Validate(
                    fieldName: nameof(address.LineThree),
                    maxLength: 100),
                address.LineFour.Validate(
                    fieldName: nameof(address.LineFour),
                    maxLength: 100),
                address.Postcode.Validate(
                    fieldName: nameof(address.Postcode),
                    maxLength: 20),
            ]);


    private static void ValidateBusiness(List<Result> results, Address address)
    {
        if(address.Postcode.StartsWith("0"))
            results.Add(Result.Failure(
                message: "Validation failed",
                errorDetails: ErrorDetails.Create(["Postcode cannot start with 0."]),
                statusCode: HttpStatusCode.UnprocessableContent));
    }

}

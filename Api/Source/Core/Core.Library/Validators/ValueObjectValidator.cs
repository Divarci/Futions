using Core.Library.ResultPattern;
using System.Net;

namespace Core.Library.Validators;

public static class ValueObjectValidator
{
    /// <summary>
    /// Validates a ValueObject type according to the given rules.
    /// </summary>
    /// <typeparam name="TValueObject">The ValueObject type to validate.</typeparam>
    /// <param name="value">The object to validate.</param>
    /// <param name="fieldName">The field name.</param>
    /// <returns>Returns Success if valid, otherwise Failure.</returns>
    public static Result Validate<TValueObject>(
        this TValueObject? value,
        string fieldName) where TValueObject : class
    {
        if (string.IsNullOrWhiteSpace(fieldName))
            return Result.Failure(
                message: $"{nameof(fieldName)} cannot be empty",
                statusCode: HttpStatusCode.InternalServerError);

        if (value is null)
            return Result.Failure(
                message: $"{fieldName} validation failed",
                errorDetails: ErrorDetails.Create([$"{fieldName} cannot be null"]),
                statusCode: HttpStatusCode.UnprocessableEntity);

        return Result.Success("Validation successful");
    }
}
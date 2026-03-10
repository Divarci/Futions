using Core.Library.ResultPattern;
using System.Net;

namespace Core.Library.Validators;

public static class GuidValidator
{
    /// <summary>
    /// Validates a nullable Guid value according to the given rules.
    /// </summary>
    /// <param name="value">The Guid value to validate (nullable).</param>
    /// <param name="fieldName">The field name.</param>
    /// <param name="isRequired">Whether the value is required.</param>
    /// <param name="allowEmpty">Whether empty Guid is allowed.</param>
    /// <returns>Returns Success if valid, otherwise Failure.</returns>
    public static Result Validate(
        this Guid? value,
        string fieldName,
        bool isRequired = true,
        bool allowEmpty = true)
    {
        if (string.IsNullOrWhiteSpace(fieldName))
            return Result.Failure(
                message: $"{nameof(fieldName)} can not be null or empty",
                statusCode: HttpStatusCode.InternalServerError);

        List<string> validationErrors = [];

        if (isRequired && value is null)
            validationErrors.Add($"{fieldName} cannot be null");

        if (value is not null && !allowEmpty && value == Guid.Empty)
            validationErrors.Add($"{fieldName} cannot be empty guid");

        return validationErrors.Count > 0
            ? Result.Failure(
                message: $"{fieldName} validation failed",
                errorDetails: ErrorDetails.Create(validationErrors),
                statusCode: HttpStatusCode.UnprocessableEntity)
            : Result.Success("Validation successful");
    }

    /// <summary>
    /// Validates a Guid value according to the given rules.
    /// </summary>
    /// <param name="value">The Guid value to validate.</param>
    /// <param name="fieldName">The field name.</param>
    /// <param name="allowEmpty">Whether empty Guid is allowed.</param>
    /// <returns>Returns Success if valid, otherwise Failure.</returns>
    public static Result Validate(
        this Guid value,
        string fieldName,
        bool allowEmpty = true)
    {
        if (string.IsNullOrWhiteSpace(fieldName))
            return Result.Failure(
                message: $"{nameof(fieldName)} cannot be empty",
                statusCode: HttpStatusCode.InternalServerError);

        List<string> validationErrors = [];

        if (!allowEmpty && value == Guid.Empty)
            validationErrors.Add($"{fieldName} cannot be empty guid");

        return validationErrors.Count > 0
            ? Result.Failure(
                message: $"{fieldName} validation failed",
                errorDetails: ErrorDetails.Create(validationErrors),
                statusCode: HttpStatusCode.UnprocessableEntity)
            : Result.Success("Validation successful");
    }
}
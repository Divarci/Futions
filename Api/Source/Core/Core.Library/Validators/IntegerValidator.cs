using Core.Library.ResultPattern;
using System.Net;

namespace Core.Library.Validators;

public static class IntegerValidator
{
    /// <summary>
    /// Validates a nullable int value according to the given rules.
    /// </summary>
    /// <param name="value">The value to validate (nullable).</param>
    /// <param name="fieldName">The field name.</param>
    /// <param name="isRequired">Whether the value is required.</param>
    /// <param name="canBeNegative">Whether the value can be negative.</param>
    /// <param name="minValue">Minimum allowed value.</param>
    /// <param name="maxValue">Maximum allowed value.</param>
    /// <returns>Returns Success if valid, otherwise Failure.</returns>
    public static Result Validate(
        this int? value,
        string fieldName,
        bool isRequired = true,
        bool canBeNegative = false,
        int? minValue = null,
        int? maxValue = null)
    {
        if (string.IsNullOrWhiteSpace(fieldName))
            return Result.Failure(
                message: $"{nameof(fieldName)} can not be null or empty",
                statusCode: HttpStatusCode.InternalServerError);

        if (minValue.HasValue && minValue.Value > int.MaxValue)
            return Result.Failure(
                message: $"{nameof(minValue)} cannot be greater than {int.MaxValue}",
                statusCode: HttpStatusCode.BadRequest);

        if (maxValue.HasValue && maxValue.Value > int.MaxValue)
            return Result.Failure(
                message: $"{nameof(maxValue)} cannot be greater than {int.MaxValue}",
                statusCode: HttpStatusCode.BadRequest);

        if (minValue.HasValue && maxValue.HasValue && minValue.Value > maxValue.Value)
            return Result.Failure(
                message: $"{nameof(minValue)} cannot be greater than {nameof(maxValue)}",
                statusCode: HttpStatusCode.BadRequest);

        List<string> validationErrors = [];

        if (isRequired && value is null)
            validationErrors.Add($"{fieldName} cannot be null");

        if (value is not null)
        {
            if (!canBeNegative && value < 0)
                validationErrors.Add($"{fieldName} cannot be negative");

            if (minValue.HasValue && value < minValue.Value)
                validationErrors.Add($"{fieldName} cannot be less than {minValue.Value}");

            if (maxValue.HasValue && value > maxValue.Value)
                validationErrors.Add($"{fieldName} cannot be greater than {maxValue.Value}");
        }

        return validationErrors.Count > 0
            ? Result.Failure(
                message: $"{fieldName} validation failed",
                errorDetails: ErrorDetails.Create(validationErrors),
                statusCode: HttpStatusCode.UnprocessableEntity)
            : Result.Success("Validation successful");
    }

    public static Result Validate(
        this int value,
        string fieldName,
        bool canBeNegative = false,
        int? minValue = null,
        int? maxValue = null)
    {
        if (string.IsNullOrWhiteSpace(fieldName))
            return Result.Failure(
                message: $"{nameof(fieldName)} cannot be empty",
                statusCode: HttpStatusCode.InternalServerError);

        if (minValue.HasValue && minValue.Value > int.MaxValue)
            return Result.Failure(
                message: $"{nameof(minValue)} cannot be greater than {int.MaxValue}",
                statusCode: HttpStatusCode.BadRequest);

        if (maxValue.HasValue && maxValue.Value > int.MaxValue)
            return Result.Failure(
                message: $"{nameof(maxValue)} cannot be greater than {int.MaxValue}",
                statusCode: HttpStatusCode.BadRequest);

        if (minValue.HasValue && maxValue.HasValue && minValue.Value > maxValue.Value)
            return Result.Failure(
                message: $"{nameof(minValue)} cannot be greater than {nameof(maxValue)}",
                statusCode: HttpStatusCode.BadRequest);

        List<string> validationErrors = [];

        if (!canBeNegative && value < 0)
            validationErrors.Add($"{fieldName} cannot be negative");

        if (minValue.HasValue && value < minValue.Value)
            validationErrors.Add($"{fieldName} cannot be less than {minValue.Value}");

        if (maxValue.HasValue && value > maxValue.Value)
            validationErrors.Add($"{fieldName} cannot be greater than {maxValue.Value}");

        return validationErrors.Count > 0
            ? Result.Failure(
                message: $"{fieldName} validation failed",
                errorDetails: ErrorDetails.Create(validationErrors),
                statusCode: HttpStatusCode.UnprocessableEntity)
            : Result.Success("Validation successful");
    }
}
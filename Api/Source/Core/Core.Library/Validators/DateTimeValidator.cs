using Core.Library.ResultPattern;
using Core.Library.Validators.Enums;
using System.Net;

namespace Core.Library.Validators;

public static class DateTimeValidator
{
    /// <summary>
    /// Validates a nullable DateTime value according to the given rules.
    /// </summary>
    /// <param name="value">The DateTime value to validate (nullable).</param>
    /// <param name="fieldName">The field name.</param>
    /// <param name="isRequired">Whether the value is required.</param>
    /// <param name="minValue">Minimum allowed value.</param>
    /// <param name="maxValue">Maximum allowed value.</param>
    /// <param name="restriction">Date restriction.</param>
    /// <returns>Returns Success if valid, otherwise Failure.</returns>
    public static Result Validate(
        this DateTime? value,
        string fieldName,
        bool isRequired = true,
        DateTime? minValue = null,
        DateTime? maxValue = null,
        DateTimeRestriction restriction = DateTimeRestriction.None)
    {
        if (string.IsNullOrWhiteSpace(fieldName))
            return Result.Failure(
                message: $"{nameof(fieldName)} can not be null or empty",
                statusCode: HttpStatusCode.InternalServerError);

        if (minValue.HasValue && minValue.Value > DateTime.MaxValue)
            return Result.Failure(
                message: $"{nameof(minValue)} cannot be greater than {DateTime.MaxValue}",
                statusCode: HttpStatusCode.BadRequest);

        if (maxValue.HasValue && maxValue.Value > DateTime.MaxValue)
            return Result.Failure(
                message: $"{nameof(maxValue)} cannot be greater than {DateTime.MaxValue}",
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
            if (restriction == DateTimeRestriction.OnlyPast && value > DateTime.UtcNow)
                validationErrors.Add($"{fieldName} cannot be a future date");

            if (restriction == DateTimeRestriction.OnlyFuture && value < DateTime.UtcNow)
                validationErrors.Add($"{fieldName} cannot be a past date");

            if (minValue.HasValue && value < minValue.Value)
                validationErrors.Add($"{fieldName} cannot be earlier than {minValue.Value:yyyy-MM-dd HH:mm:ss}");

            if (maxValue.HasValue && value > maxValue.Value)
                validationErrors.Add($"{fieldName} cannot be later than {maxValue.Value:yyyy-MM-dd HH:mm:ss}");
        }

        return validationErrors.Count > 0
            ? Result.Failure(
                message: $"{fieldName} validation failed",
                errorDetails: ErrorDetails.Create(validationErrors),
                statusCode: HttpStatusCode.UnprocessableEntity)
            : Result.Success(
                message: "Validation successful",
                statusCode: HttpStatusCode.OK);
    }

    /// <summary>
    /// Validates a DateTime value according to the given rules.
    /// </summary>
    /// <param name="value">The DateTime value to validate.</param>
    /// <param name="fieldName">The field name.</param>
    /// <param name="minValue">Minimum allowed value.</param>
    /// <param name="maxValue">Maximum allowed value.</param>
    /// <param name="restriction">Date restriction.</param>
    /// <returns>Returns Success if valid, otherwise Failure.</returns>
    public static Result Validate(
        this DateTime value,
        string fieldName,
        DateTime? minValue = null,
        DateTime? maxValue = null,
        DateTimeRestriction restriction = DateTimeRestriction.None)
    {
        if (string.IsNullOrWhiteSpace(fieldName))
            return Result.Failure(
                message: $"{nameof(fieldName)} cannot be empty",
                statusCode: HttpStatusCode.InternalServerError);

        if (minValue.HasValue && minValue.Value > DateTime.MaxValue)
            return Result.Failure(
                message: $"{nameof(minValue)} cannot be greater than {DateTime.MaxValue}",
                statusCode: HttpStatusCode.BadRequest);

        if (maxValue.HasValue && maxValue.Value > DateTime.MaxValue)
            return Result.Failure(
                message: $"{nameof(maxValue)} cannot be greater than {DateTime.MaxValue}",
                statusCode: HttpStatusCode.BadRequest);

        if (minValue.HasValue && maxValue.HasValue && minValue.Value > maxValue.Value)
            return Result.Failure(
                message: $"{nameof(minValue)} cannot be greater than {nameof(maxValue)}",
                statusCode: HttpStatusCode.BadRequest);

        List<string> validationErrors = [];

        if (restriction == DateTimeRestriction.OnlyPast && value > DateTime.UtcNow)
            validationErrors.Add($"{fieldName} cannot be a future date");

        if (restriction == DateTimeRestriction.OnlyFuture && value < DateTime.UtcNow)
            validationErrors.Add($"{fieldName} cannot be a past date");

        if (minValue.HasValue && value < minValue.Value)
            validationErrors.Add($"{fieldName} cannot be earlier than {minValue.Value:yyyy-MM-dd HH:mm:ss}");

        if (maxValue.HasValue && value > maxValue.Value)
            validationErrors.Add($"{fieldName} cannot be later than {maxValue.Value:yyyy-MM-dd HH:mm:ss}");

        return validationErrors.Count > 0
            ? Result.Failure(
                message: $"{fieldName} validation failed",
                errorDetails: ErrorDetails.Create(validationErrors),
                statusCode: HttpStatusCode.UnprocessableEntity)
            : Result.Success(
                message: "Validation successful",
                statusCode: HttpStatusCode.OK);
    }
}
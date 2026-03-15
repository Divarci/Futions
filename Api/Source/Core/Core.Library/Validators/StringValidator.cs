using Core.Library.ResultPattern;
using System.Net;
using System.Text.RegularExpressions;

namespace Core.Library.Validators;

public static class StringValidator
{
    private static readonly Regex EmailRegex = new(
        @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
        RegexOptions.Compiled | RegexOptions.IgnoreCase);

    /// <summary>
    /// Validates a string value according to the given rules.
    /// </summary>
    /// <param name="value">The string value to validate (nullable).</param>
    /// <param name="fieldName">The field name.</param>
    /// <param name="maxLength">Maximum allowed length.</param>
    /// <param name="minLength">Minimum allowed length.</param>
    /// <param name="isRequired">Whether the value is required.</param>
    /// <param name="isEmail">Whether to check for email format.</param>
    /// <returns>Returns Success if valid, otherwise Failure.</returns>
    public static Result Validate(
        this string? value,
        string fieldName,
        int maxLength,
        int minLength = 0,
        bool isRequired = true,
        bool isEmail = false)
    {
        List<string> validationErrors = [];

        if (isRequired && string.IsNullOrWhiteSpace(value))
            validationErrors.Add($"{fieldName} cannot be null or empty.");

        if (!string.IsNullOrWhiteSpace(value))
        {
            if (minLength > 0 && value.Length < minLength)
                validationErrors.Add($"{fieldName} must be at least {minLength} characters.");

            if (value.Length > maxLength)
                validationErrors.Add($"{fieldName} must be {maxLength} characters or less.");

            if (isEmail && !EmailRegex.IsMatch(value))
                validationErrors.Add($"{fieldName} must be a valid email address.");
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
}
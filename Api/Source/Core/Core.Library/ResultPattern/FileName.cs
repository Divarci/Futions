using System.Net;

namespace Core.Library.ResultPattern;

public static class Extensions
{
    /// <summary>
    /// Combines multiple validation results into a single Result
    /// </summary>
    public static Result CombineAndValidate(this IEnumerable<Result> results, string? validationMessage = null)
    {
        List<string> errors = [];

        foreach (Result result in results)
        {
            if (result.IsFailure && result.ErrorDetails != null)
                errors.AddRange(result.ErrorDetails.Errors.Count == 0 ? [result.Message] : result.ErrorDetails.Errors);
        }

        return errors.Count > 0
            ? Result.Failure(
                message: validationMessage ?? "Result Merge Failed",
                statusCode: HttpStatusCode.InternalServerError
            )
            : Result.Success(validationMessage ?? "Result Merge Succeeded");
    }
}


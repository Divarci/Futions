using Core.Library.ResultPattern;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Adapter.RestApi.AspNetCore.Helpers;

public static class ApiResultHelper
{
    /// <summary>
    /// Builds a <see cref="ProblemDetails"/> object from a failed <see cref="Result"/>.
    /// Follows RFC 7807 (Problem Details for HTTP APIs).
    /// </summary>
    public static ProblemDetails Problem(Result result)
    {
        if (result.IsSuccess)
            throw new InvalidOperationException("Problem can only be built from a failed result with a status code.");

        ProblemDetails problemDetails = new()
        {
            Title = result.Message,
            Type = GetRfcType(result.StatusCode),
            Status = (int)result.StatusCode
        };

        if (result.ErrorDetails?.Errors is not null)
            problemDetails.Extensions["errors"] = result.ErrorDetails.Errors;

        return problemDetails;

        static string GetRfcType(HttpStatusCode statusCode) =>
            statusCode switch
            {
                HttpStatusCode.NotFound => "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.4",
                HttpStatusCode.BadRequest => "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1",
                HttpStatusCode.Unauthorized => "https://datatracker.ietf.org/doc/html/rfc7235#section-3.1",
                HttpStatusCode.Forbidden => "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.3",
                HttpStatusCode.UnprocessableEntity => "https://datatracker.ietf.org/doc/html/rfc4918#section-11.2",
                _ => "https://datatracker.ietf.org/doc/html/rfc7231#section-6.6.1",
            };
    }
}

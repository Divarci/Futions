using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Adapter.RestApi.AspNetCore.Diagnostics;

public sealed partial class GlobalExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        string traceId = Guid.NewGuid().ToString();

        ProblemDetails problemDetails = GetExceptionDetails(exception, traceId);

        httpContext.Response.StatusCode = (int)problemDetails.Status!;
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }    

    private static ProblemDetails GetExceptionDetails(Exception exception, string traceId) =>
        exception switch
        {
            BadHttpRequestException badHttpEx when badHttpEx.InnerException is JsonException jsonEx =>
                new ProblemDetails
                {
                    Title = "Bad Request",
                    Detail = "Invaid request format.",
                    Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                    Status = StatusCodes.Status400BadRequest,
                    Extensions =
                    {
                        ["traceId"] = traceId,
                        ["errors"] = new Dictionary<string, string[]>
                        {
                            { "request", new[] { GetFriendlyJsonMessage(jsonEx) } }
                        }
                    }
                },

            _ =>
                new ProblemDetails
                {
                    Title = "Internal Server Error",
                    Detail = "An unexpected error occurred while processing your request.",
                    Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1",
                    Status = StatusCodes.Status500InternalServerError,
                    Extensions =
                    {
                        ["traceId"] = traceId,
                    }
                }
        };

    private static string GetFriendlyJsonMessage(JsonException ex)
    {
        if (ex.Message.Contains("unmapped"))
            return "This property is not recognized. Please check the API documentation.";

        if (ex.Message.Contains("required"))
            return "A required property is missing.";

        return "Invalid value or format for this property.";
    }
}
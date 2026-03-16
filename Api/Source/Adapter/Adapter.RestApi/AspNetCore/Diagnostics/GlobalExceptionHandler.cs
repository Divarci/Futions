using Core.Library.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Adapter.RestApi.AspNetCore.Diagnostics;

public sealed partial class GlobalExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        string traceId = Guid.NewGuid().ToString();

        LogException(exception, traceId);

        httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
        await httpContext.Response.WriteAsJsonAsync(BuildProblemDetails(traceId), cancellationToken);

        return true;
    }

    private static ProblemDetails BuildProblemDetails(string traceId) =>
        new()
        {
            Title = "Internal Server Error",
            Detail = "An unexpected error occurred while processing your request.",
            Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1",
            Status = StatusCodes.Status500InternalServerError,
            Extensions =
            {
                ["traceId"] = traceId,
            }
        };

    private static void LogException(Exception exception, string traceId)
    {
        switch (exception)
        {
            case FutionsException futionsEx:
                // Logging will be implemented soon
                break;

            default:
                // Logging will be implemented soon
                break;
        }
    }
}
using Adapter.RestApi.AspNetCore.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Adapter.RestApi.AspNetCore.Filters;

/// <summary>
/// Action filter that validates the tenantId in the route matches the authenticated user's tenantId from JWT token.
/// Returns 404 if tenantId does not match to prevent information disclosure.
/// </summary>
public sealed class TenantAuthorizationFilter : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
        if (context.HttpContext.IsInRole("SystemAdmin"))
            return;

        if (!context.HttpContext.Request.RouteValues.TryGetValue("tenantId", out object? routeTenantIdObj))
            return;

        if (routeTenantIdObj is not string routeTenantIdStr ||
            !Guid.TryParse(routeTenantIdStr, out Guid routeTenantId))
        {
            ProblemDetails problemDetails = new ProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Invalid Tenant ID",
                Detail = "The tenant ID in the route is not a valid GUID."
            };

            context.Result = new ObjectResult(problemDetails)
            {
                StatusCode = StatusCodes.Status400BadRequest
            };
            return;
        }

        Guid? tokenTenantId = context.HttpContext.GetTenantId();

        if (tokenTenantId is null)
        {
            ProblemDetails problemDetails = new ProblemDetails
            {
                Status = StatusCodes.Status401Unauthorized,
                Title = "Unauthorized",
                Detail = "Tenant information is missing from authentication token."
            };

            context.Result = new ObjectResult(problemDetails)
            {
                StatusCode = StatusCodes.Status401Unauthorized
            };

            return;
        }

        if (routeTenantId != tokenTenantId.Value)
        {
            ProblemDetails problemDetails = new ProblemDetails
            {
                Status = StatusCodes.Status404NotFound,
                Title = "Not Found",
                Detail = "The requested resource was not found."
            };

            context.Result = new ObjectResult(problemDetails)
            {
                StatusCode = StatusCodes.Status404NotFound
            };

            return;
        }
    }

    public void OnActionExecuted(ActionExecutedContext context) { }
}
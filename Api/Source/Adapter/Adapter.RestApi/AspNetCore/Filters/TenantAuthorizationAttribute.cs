using Adapter.RestApi.AspNetCore.Authentication;
using Adapter.RestApi.AspNetCore.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Adapter.RestApi.AspNetCore.Filters;

/// <summary>
/// Action filter attribute that validates the tenantId in the route matches
/// the authenticated user's tenantId from the JWT token.
/// Returns 404 if tenantId does not match to prevent information disclosure.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public sealed class TenantAuthorizationAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (context.HttpContext.IsInRole(Role.SystemAdminRole))
            return;

        if (!context.HttpContext.Request.RouteValues.TryGetValue("tenantId", out object? routeTenantIdObj))
            return;

        if (routeTenantIdObj is not string routeTenantIdStr ||
            !Guid.TryParse(routeTenantIdStr, out Guid routeTenantId))
        {
            context.Result = new ObjectResult(new ProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Invalid Tenant ID",
                Detail = "The tenant ID in the route is not a valid GUID."
            })
            {
                StatusCode = StatusCodes.Status400BadRequest
            };
            return;
        }

        Guid? tokenTenantId = context.HttpContext.GetTenantId();

        if (tokenTenantId is null)
        {
            context.Result = new ObjectResult(new ProblemDetails
            {
                Status = StatusCodes.Status401Unauthorized,
                Title = "Unauthorized",
                Detail = "Tenant information is missing from authentication token."
            })
            {
                StatusCode = StatusCodes.Status401Unauthorized
            };
            return;
        }

        if (routeTenantId != tokenTenantId.Value)
        {
            context.Result = new ObjectResult(new ProblemDetails
            {
                Status = StatusCodes.Status404NotFound,
                Title = "Not Found",
                Detail = "The requested resource was not found."
            })
            {
                StatusCode = StatusCodes.Status404NotFound
            };
        }
    }
}

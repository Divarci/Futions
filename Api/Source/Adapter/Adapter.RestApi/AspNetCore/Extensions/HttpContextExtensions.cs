using System.Security.Claims;

namespace Adapter.RestApi.AspNetCore.Extensions;

public static class HttpContextExtensions
{
    /// <summary>
    /// Retrieves the tenant ID from the current HTTP context.
    /// </summary>
    /// <param name="httpContext">The HTTP context.</param>
    /// <returns>The tenant ID if available; otherwise, null.</returns>
    public static Guid? GetTenantId(this HttpContext httpContext)
    {
        string? tenantIdClaim = httpContext?.User?
            .FindFirst("tenantId")?.Value;

        if (string.IsNullOrEmpty(tenantIdClaim))
            return null;

        return Guid.TryParse(tenantIdClaim, out Guid tenantId)
            ? tenantId
            : null;
    }

    /// <summary>
    /// Retrieves the user ID from the current HTTP context.
    /// </summary>
    /// <param name="httpContext">The HTTP context.</param>
    /// <returns>The user ID if available; otherwise, null.</returns>
    public static Guid? GetUserId(this HttpContext httpContext)
    {
        string? userIdClaim = httpContext?.User?
            .FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userIdClaim))
            return null;

        return Guid.TryParse(userIdClaim, out Guid userId)
            ? userId
            : null;
    }


    /// <summary>
    /// Checks if the current user is in the specified role.
    /// </summary>
    /// <param name="httpContext">The HTTP context.</param>
    /// <param name="roleName">The name of the role to check.</param>
    /// <returns>True if the user is in the role; otherwise, false.</returns>
    public static bool IsInRole(this HttpContext httpContext, string roleName)
    {
        return httpContext?.User?.IsInRole(roleName) ?? false;
    }
}

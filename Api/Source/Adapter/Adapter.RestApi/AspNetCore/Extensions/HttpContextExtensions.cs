using System.Security.Claims;

namespace Adapter.RestApi.AspNetCore.Extensions;

public static class HttpContextExtensions
{
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

    public static bool IsInRole(this HttpContext httpContext, string roleName)
    {
        return httpContext?.User?.IsInRole(roleName) ?? false;
    }
}

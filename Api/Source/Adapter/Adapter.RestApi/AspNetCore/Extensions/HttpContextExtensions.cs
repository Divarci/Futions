using Core.Library.Exceptions;
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
    /// Retrieves the tenant ID from an authenticated HTTP context.
    /// </summary>
    /// <param name="httpContext">The HTTP context.</param>
    /// <returns>The tenant ID.</returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown when the tenantId claim is missing or invalid.
    /// </exception>
    public static Guid GetRequiredTenantId(this HttpContext httpContext)
    {
        return httpContext.GetTenantId()
            ?? throw new FutionsException(
                assemblyName: "Adapter.RestApi",
                className: nameof(HttpContextExtensions),
                methodName: nameof(GetRequiredTenantId),
                message: "TenantId claim is missing or invalid. Ensure the endpoint is protected with [Authorize].");
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
    /// Retrieves the user ID from an authenticated HTTP context.
    /// </summary>
    /// <param name="httpContext">The HTTP context.</param>
    /// <returns>The user ID.</returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown when the NameIdentifier claim is missing or invalid.
    /// </exception>
    public static Guid GetRequiredUserId(this HttpContext httpContext)
    {
        return httpContext.GetUserId()
            ?? throw new FutionsException(
                assemblyName: "Adapter.RestApi",
                className: nameof(HttpContextExtensions),
                methodName: nameof(GetRequiredUserId),
                message: "UserId claim is missing or invalid. Ensure the endpoint is protected with [Authorize].");
    }

    /// <summary>
    /// Retrieves the user email from the current HTTP context.
    /// </summary>
    /// <param name="httpContext">The HTTP context.</param>
    /// <returns>The user email if available; otherwise, null.</returns>
    public static string? GetUserEmail(this HttpContext httpContext)
    {
        return httpContext?.User?
            .FindFirst(ClaimTypes.Email)?.Value;
    }

    /// <summary>
    /// Retrieves the user email from an authenticated HTTP context.
    /// </summary>
    /// <param name="httpContext">The HTTP context.</param>
    /// <returns>The user email.</returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown when the Email claim is missing.
    /// </exception>
    public static string GetRequiredUserEmail(this HttpContext httpContext)
    {
        return httpContext.GetUserEmail()
            ?? throw new FutionsException(
                assemblyName: "Adapter.RestApi",
                className: nameof(HttpContextExtensions),
                methodName: nameof(GetRequiredUserEmail),
                message: "Email claim is missing. Ensure the endpoint is protected with [Authorize].");
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
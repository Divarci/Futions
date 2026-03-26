using Adapter.RestApi.AspNetCore.Authentication;
using Adapter.RestApi.AspNetCore.Diagnostics;
using Adapter.RestApi.AspNetCore.Filters;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using System.Threading.RateLimiting;

namespace Adapter.RestApi;

public static class ServiceRegistrar
{
    private const string CorsPolicyName = "FrontendPolicy";

    public static IServiceCollection RegisterRestApiLayer(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers(options => options.Filters.Add<ValidationFilter>());
        services.Configure<ApiBehaviorOptions>(options => options.SuppressModelStateInvalidFilter = true);
        services.AddOpenApi();
        services.AddCorsInternal(configuration);
        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails();
        services.AddApiVersioning();
        services.AddAuthenticationInternal();
        services.AddRateLimiting();

        return services;
    }

    private static IServiceCollection AddCorsInternal(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        string[] allowedOrigins = configuration
            .GetSection("Cors:AllowedOrigins")
            .Get<string[]>() ?? [];

        services.AddCors(options =>
            options.AddPolicy(CorsPolicyName, policy =>
                policy
                    .WithOrigins(allowedOrigins)
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials()));

        return services;
    }

    private static IServiceCollection AddApiVersioning(
        this IServiceCollection services)
    {
        services
            .AddApiVersioning(opt =>
            {
                opt.DefaultApiVersion = new ApiVersion(1,0);
                opt.ReportApiVersions = true;
                opt.ApiVersionReader = new UrlSegmentApiVersionReader();
            })
            .AddMvc();
        return services;
    }

    private static IServiceCollection AddAuthenticationInternal(
        this IServiceCollection services)
    {
        services.AddAuthorizationBuilder()
            .AddPolicy(PolicyNames.AllRoles, policy =>
                policy.RequireRole(Role.UserRole, Role.AdminRole, Role.SystemAdminRole))
            .AddPolicy(PolicyNames.AdminOrSystemAdmin, policy =>
                policy.RequireRole(Role.AdminRole, Role.SystemAdminRole))
            .AddPolicy(PolicyNames.SystemAdmin, policy =>
                policy.RequireRole(Role.SystemAdminRole));

        services.AddAuthentication().AddJwtBearer();
        services.ConfigureOptions<JwtBearerConfigurationOptions>();
        services.AddHttpContextAccessor();

        return services;
    }

    private static IServiceCollection AddRateLimiting(this IServiceCollection services)
    {
        services.AddRateLimiter(options =>
        {
            options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
                RateLimitPartition.GetFixedWindowLimiter(
                    partitionKey: httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown",
                    factory: _ => new FixedWindowRateLimiterOptions
                    {
                        Window = TimeSpan.FromSeconds(10),
                        PermitLimit = 10,
                        QueueLimit = 0,
                    }));

            options.OnRejected = async (context, token) =>
            {
                string traceId = Guid.NewGuid().ToString();

                context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                await context.HttpContext.Response.WriteAsJsonAsync(
                    new ProblemDetails
                    {
                        Title = "Too Many Requests",
                        Detail = "You have exceeded the allowed request limit. Please try again later.",
                        Type = "https://tools.ietf.org/html/rfc6585#section-4",
                        Status = StatusCodes.Status429TooManyRequests,
                        Extensions =
                        {
                            ["traceId"] = traceId,
                        }
                    }, token);
            };
        });

        return services;
    }
}
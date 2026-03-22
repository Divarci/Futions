using Adapter.RestApi.AspNetCore.Authentication;
using Adapter.RestApi.AspNetCore.Diagnostics;
using Adapter.RestApi.AspNetCore.Filters;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace Adapter.RestApi;

public static class ServiceRegistrar
{
    public static IServiceCollection RegisterRestApiLayer(this IServiceCollection services)
    {
        services.AddControllers(options => options.Filters.Add<ValidationFilter>());
        services.Configure<ApiBehaviorOptions>(options => options.SuppressModelStateInvalidFilter = true);
        services.AddOpenApi();
        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails();
        services.AddApiVersioning();
        services.AddAuthenticationInternal();

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

    //private static IServiceCollection AddSerilog
}

using Adapter.RestApi.AspNetCore.Diagnostics;
using Asp.Versioning;

namespace Adapter.RestApi;

public static class ServiceRegistrar
{
    public static IServiceCollection RegisterRestApiLayer(this IServiceCollection services)
    {
        services.AddControllers();
        services.AddOpenApi();
        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails();
        services.AddApiVersioning();

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
}

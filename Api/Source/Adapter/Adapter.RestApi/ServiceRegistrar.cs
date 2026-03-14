using Adapter.RestApi.AspNetCore.Diagnostics;

namespace Adapter.RestApi;

public static class ServiceRegistrar
{
    public static IServiceCollection RegisterRestApiLayer(this IServiceCollection services)
    {
        services.AddControllers();
        services.AddOpenApi();
        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails();

        return services;
    }
}

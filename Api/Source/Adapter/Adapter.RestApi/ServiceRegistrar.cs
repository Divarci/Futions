namespace Adapter.RestApi;

public static class ServiceRegistrar
{
    public static IServiceCollection RegisterRestApiLayer(this IServiceCollection services)
    {
        services.AddControllers();
        services.AddOpenApi();

        return services;
    }
}

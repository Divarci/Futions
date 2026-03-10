using Microsoft.Extensions.DependencyInjection;

namespace App.UseCases;

public static class ServiceRegistrar
{
    public static IServiceCollection RegisterUseCaseLayer(this IServiceCollection services)
    {        
        return services;
    }
}

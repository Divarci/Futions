using Microsoft.Extensions.DependencyInjection;

namespace Infra.Persistence;

public static class ServiceRegistrar
{
    public static IServiceCollection RegisterPersistenceLayer(this IServiceCollection services)
    {        
        return services;
    }
}

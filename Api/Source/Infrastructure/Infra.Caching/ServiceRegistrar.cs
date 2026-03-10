using Microsoft.Extensions.DependencyInjection;

namespace Infra.Caching;

public static class ServiceRegistrar
{
    public static IServiceCollection RegisterCachingLayer(this IServiceCollection services)
    {        
        return services;
    }
}

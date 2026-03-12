using Core.Library.Contracts.Caching;
using Infra.Caching.Configuration;
using Infra.Caching.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace Infra.Caching;

public static class ServiceRegistrar
{
    public static IServiceCollection RegisterCachingLayer(this IServiceCollection services, IConfiguration configuration)
    {        
        services.RegisterCacheConfig(configuration);

        services.RegisterCacheService();

        return services;
    }

    private static void RegisterCacheConfig(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<CacheConfig>(configuration.GetSection(nameof(CacheConfig)));

        services.AddSingleton(sp => sp.GetRequiredService<IOptions<CacheConfig>>().Value);
    }

    private static void RegisterCacheService(this IServiceCollection services)
    {
        services.AddSingleton(sp =>
        {
            CacheConfig config = sp.GetRequiredService<CacheConfig>();

            return ConnectionMultiplexer.Connect(new ConfigurationOptions
            {
                EndPoints = { config.HostAndPort },
                Ssl = config.Ssl,
                ConnectTimeout = config.Timeout,
                SyncTimeout = config.Timeout,
                AbortOnConnectFail = false,
                Password = config.AccessKey
            });
        });

        services.AddSingleton<CacheService>();
        services.AddSingleton<ICacheProvider>(sp => sp.GetRequiredService<CacheService>());
        services.AddSingleton<ICacheInvalidationService>(sp => sp.GetRequiredService<CacheService>());
    }
}

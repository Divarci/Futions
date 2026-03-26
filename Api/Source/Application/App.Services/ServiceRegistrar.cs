using App.Services.Features.System.AuditLogs;
using App.Services.Features.System.OutboxMessages;
using Core.Domain.Entities.System.AuditLogs.Interfaces;
using Core.Domain.Entities.System.OutboxMessages.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace App.Services;

public static class ServiceRegistrar
{
    public static IServiceCollection RegisterServiceLayer(this IServiceCollection services)
    {
        services.RegisterServices();

        return services;
    }

    private static void RegisterServices(this IServiceCollection services)
    {       
        services.AddScoped<IAuditLogService, AuditLogService>();
        services.AddScoped<IOutboxMessageService, OutboxMessageService>();
    }
}

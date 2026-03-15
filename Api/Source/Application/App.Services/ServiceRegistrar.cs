using App.Services.Features.Organisations.Companies;
using App.Services.Features.Organisations.People;
using Core.Domain.Entities.Organisations.Companies.Interfaces;
using Core.Domain.Entities.Organisations.CompanyPeople.Interfaces;
using Core.Domain.Entities.Organisations.People.Interfaces;
using Core.Domain.Entities.Organisations.Products.Interfaces;
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
        services.AddScoped<ICompanyService, CompanyService>();
        services.AddScoped<IPersonService, PersonService>();
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<ICompanyPersonService, CompanyPersonService>();
        services.AddScoped<IAuditLogService, AuditLogService>();
        services.AddScoped<IOutboxMessageService, OutboxMessageService>();
    }
}

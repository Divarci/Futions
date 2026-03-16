using App.UseCases.Processors;
using App.UseCases.UseCases.Organisations.Companies;
using App.UseCases.UseCases.Organisations.CompanyPeople;
using App.UseCases.UseCases.Organisations.People;
using App.UseCases.UseCases.System.AuditLogs;
using Core.Domain.Entities.Organisations.Companies.Interfaces;
using Core.Domain.Entities.Organisations.CompanyPeople.Interfaces;
using Core.Domain.Entities.Organisations.People.Interfaces;
using Core.Domain.Entities.System.AuditLogs.Interfaces;
using Core.Domain.Entities.System.OutboxMessages.Interfaces;
using Core.Library.Contracts.DomainEvents.Handle;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Reflection;

namespace App.UseCases;

public static class ServiceRegistrar
{
    public static Assembly Assembly => typeof(ServiceRegistrar).Assembly;

    public static IServiceCollection RegisterUseCaseLayer(this IServiceCollection services)
    {
        services.AddDomainEventHandlers();

        services.RegisterUseCases();

        return services;
    }

    private static IServiceCollection AddDomainEventHandlers(this IServiceCollection services)
    {
        Type[] domainEventHandlers = [.. Assembly
        .GetTypes()
        .Where(t => t.IsAssignableTo(typeof(IDomainEventHandler)))];

        foreach (Type domainEventHandler in domainEventHandlers)
            services.TryAddScoped(domainEventHandler);

        return services;
    }

    private static void RegisterUseCases(this IServiceCollection services)
    {
        services.AddScoped<ICompanyUseCase, CompanyUseCase>();
        services.AddScoped<IPersonUseCase, PersonUseCase>();
        services.AddScoped<ICompanyPersonUseCase, CompanyPersonUseCase>();
        services.AddScoped<IAuditLogUseCase, AuditLogUseCase>();
        services.AddScoped<IOutboxProcessor, OutboxProcessor>();
    }
}

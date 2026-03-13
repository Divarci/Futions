using Core.Domain.Entities.Organisations.Companies.Interfaces;
using Core.Domain.Entities.Organisations.CompanyPeople.Interfaces;
using Core.Domain.Entities.Organisations.People.Interfaces;
using Core.Domain.Entities.Organisations.Products.Interfaces;
using Core.Domain.Entities.System.AuditLogs.Interfaces;
using Core.Domain.Entities.System.OutboxMessages.Interfaces;
using Core.Library.Contracts.GenericRepositories;
using Core.Library.Contracts.UnitOfWorks;
using Infra.Persistence.Repositories.Generics;
using Infra.Persistence.Repositories.Organisations.Companies;
using Infra.Persistence.Repositories.Organisations.CompanyPeople;
using Infra.Persistence.Repositories.Organisations.People;
using Infra.Persistence.Repositories.Organisations.Products;
using Infra.Persistence.Repositories.System.AuditLogs;
using Infra.Persistence.Repositories.System.OutboxMessages;
using Infra.Persistence.UnitOfWorks;
using Microsoft.Extensions.DependencyInjection;

namespace Infra.Persistence;

public static class ServiceRegistrar
{
    public static IServiceCollection RegisterPersistenceLayer(this IServiceCollection services)
    {        
        services.RegisterRepositories();

        return services;
    }

    private static void RegisterRepositories(this IServiceCollection services)
    {
        services.AddScoped(typeof(IGlobalRepository<>), typeof(GlobalRepository<>));
        services.AddScoped(typeof(ITenantedRepository<>), typeof(TenantedRepository<>));

        services.AddScoped<ITransactionalUnitOfWork, UnitOfWork>();

        services.AddScoped<ICompanyRepository, CompanyRepository>();
        services.AddScoped<IPersonRepository, PersonRepository>();
        services.AddScoped<ICompanyPersonRepository, CompanyPersonRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IAuditLogRepository, AuditLogRepository>();
        services.AddScoped<IOutboxMessageRepository, OutboxMessageRepository>();
    }
}

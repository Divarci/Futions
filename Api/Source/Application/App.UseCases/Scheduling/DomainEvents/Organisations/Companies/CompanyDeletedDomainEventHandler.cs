using Core.Domain.Entities.Organisations.Companies.DomainEvents;
using Core.Library.Contracts.DomainEvents.Handle;
using Core.Library.Exceptions;

namespace App.UseCases.Scheduling.DomainEvents.Organisations.Companies;

internal sealed class CompanyDeletedDomainEventHandler : DomainEventHandler<CompanyDeletedDomainEvent>
{
    public override Task Handle(CompanyDeletedDomainEvent domainEvent, CancellationToken cancellationToken = default)
    {
        throw new FutionsException(
            assemblyName: "App.UseCases",
            className: nameof(CompanyDeletedDomainEventHandler),
            methodName: nameof(Handle),
            message: "Handle method is not implemented.");
    }
}

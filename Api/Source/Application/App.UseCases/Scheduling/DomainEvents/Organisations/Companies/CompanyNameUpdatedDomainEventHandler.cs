using Core.Domain.Entities.Organisations.Companies.DomainEvents;
using Core.Library.Contracts.DomainEvents.Handle;
using Core.Library.Exceptions;

namespace App.UseCases.Scheduling.DomainEvents.Organisations.Companies;

internal sealed class CompanyNameUpdatedDomainEventHandler : DomainEventHandler<CompanyNameUpdatedDomainEvent>
{
    public override Task Handle(CompanyNameUpdatedDomainEvent domainEvent, CancellationToken cancellationToken = default)
    {
        throw new FutionsException(
            assemblyName: "App.UseCases",
            className: nameof(CompanyNameUpdatedDomainEventHandler),
            methodName: nameof(Handle),
            message: "Handle method is not implemented.");
    }
}

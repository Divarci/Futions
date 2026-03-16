using Core.Domain.Entities.Organisations.CompanyPeople.DomainEvents;
using Core.Library.Contracts.DomainEvents.Handle;
using Core.Library.Exceptions;

namespace App.UseCases.Scheduling.DomainEvents.Organisations.CompanyPeople;

internal sealed class CompanyPersonCreatedDomainEventHandler : DomainEventHandler<CompanyPersonCreatedDomainEvent>
{
    public override Task Handle(CompanyPersonCreatedDomainEvent domainEvent, CancellationToken cancellationToken = default)
    {
        throw new FutionsException(
            assemblyName: "App.UseCases",
            className: nameof(CompanyPersonCreatedDomainEventHandler),
            methodName: nameof(Handle),
            message: "Handle method is not implemented.");
    }
}

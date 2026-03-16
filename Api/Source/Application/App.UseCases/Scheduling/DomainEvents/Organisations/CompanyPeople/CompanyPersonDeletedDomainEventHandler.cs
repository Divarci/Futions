using Core.Domain.Entities.Organisations.CompanyPeople.DomainEvents;
using Core.Library.Contracts.DomainEvents.Handle;
using Core.Library.Exceptions;

namespace App.UseCases.Scheduling.DomainEvents.Organisations.CompanyPeople;

internal sealed class CompanyPersonDeletedDomainEventHandler : DomainEventHandler<CompanyPersonDeletedDomainEvent>
{
    public override Task Handle(CompanyPersonDeletedDomainEvent domainEvent, CancellationToken cancellationToken = default)
    {
        throw new FutionsException(
            assemblyName: "App.UseCases",
            className: nameof(CompanyPersonDeletedDomainEventHandler),
            methodName: nameof(Handle),
            message: "Handle method is not implemented.");
    }
}

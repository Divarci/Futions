using Core.Domain.Entities.Organisations.CompanyPeople.DomainEvents;
using Core.Library.Contracts.DomainEvents.Handle;

namespace App.UseCases.Scheduling.DomainEvents.Organisations.CompanyPeople;

internal sealed class CompanyPersonCreatedDomainEventHandler : DomainEventHandler<CompanyPersonCreatedDomainEvent>
{
    public override Task Handle(CompanyPersonCreatedDomainEvent domainEvent, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}

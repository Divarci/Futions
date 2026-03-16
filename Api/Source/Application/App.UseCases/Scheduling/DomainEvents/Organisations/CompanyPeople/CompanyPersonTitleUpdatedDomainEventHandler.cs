using Core.Domain.Entities.Organisations.CompanyPeople.DomainEvents;
using Core.Library.Contracts.DomainEvents.Handle;

namespace App.UseCases.Scheduling.DomainEvents.Organisations.CompanyPeople;

internal sealed class CompanyPersonTitleUpdatedDomainEventHandler : DomainEventHandler<CompanyPersonTitleUpdatedDomainEvent>
{
    public override Task Handle(CompanyPersonTitleUpdatedDomainEvent domainEvent, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}

using Core.Domain.Entities.Organisations.CompanyPeople.DomainEvents;
using Core.Library.Contracts.DomainEvents.Handle;

namespace App.UseCases.Scheduling.DomainEvents.Organisations.CompanyPeople;

internal sealed class CompanyPersonDeletedDomainEventHandler : DomainEventHandler<CompanyPersonDeletedDomainEvent>
{
    public override Task Handle(CompanyPersonDeletedDomainEvent domainEvent, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}

using Core.Domain.Entities.Organisations.Companies.DomainEvents;
using Core.Library.Contracts.DomainEvents.Handle;

namespace App.UseCases.Scheduling.DomainEvents.Organisations.Companies;

internal sealed class CompanyAddressUpdatedDomainEventHandler : DomainEventHandler<CompanyAddressUpdatedDomainEvent>
{
    public override Task Handle(CompanyAddressUpdatedDomainEvent domainEvent, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}

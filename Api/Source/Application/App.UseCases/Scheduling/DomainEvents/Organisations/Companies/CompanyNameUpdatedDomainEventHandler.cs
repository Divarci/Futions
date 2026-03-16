using Core.Domain.Entities.Organisations.Companies.DomainEvents;
using Core.Library.Contracts.DomainEvents.Handle;

namespace App.UseCases.Scheduling.DomainEvents.Organisations.Companies;

internal sealed class CompanyNameUpdatedDomainEventHandler : DomainEventHandler<CompanyNameUpdatedDomainEvent>
{
    public override Task Handle(CompanyNameUpdatedDomainEvent domainEvent, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}

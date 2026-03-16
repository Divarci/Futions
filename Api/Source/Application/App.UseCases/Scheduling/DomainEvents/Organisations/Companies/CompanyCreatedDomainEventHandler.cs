using Core.Domain.Entities.Organisations.Companies.DomainEvents;
using Core.Library.Contracts.DomainEvents.Handle;

namespace App.UseCases.Scheduling.DomainEvents.Organisations.Companies;

internal sealed class CompanyCreatedDomainEventHandler : DomainEventHandler<CompanyCreatedDomainEvent>
{
    public override Task Handle(CompanyCreatedDomainEvent domainEvent, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}

using Core.Domain.Entities.Organisations.Companies.DomainEvents;
using Core.Library.Contracts.DomainEvents.Handle;

namespace App.UseCases.Scheduling.DomainEvents.Organisations.Companies;

internal sealed class CompanyDeletedDomainEventHandler : DomainEventHandler<CompanyDeletedDomainEvent>
{
    public override Task Handle(CompanyDeletedDomainEvent domainEvent, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}

using Core.Domain.Entities.Organisations.People.DomainEvents;
using Core.Library.Contracts.DomainEvents.Handle;

namespace App.UseCases.Scheduling.DomainEvents.Organisations.People;

internal sealed class PersonDeletedDomainEventHandler : DomainEventHandler<PersonDeletedDomainEvent>
{
    public override Task Handle(PersonDeletedDomainEvent domainEvent, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}

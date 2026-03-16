using Core.Domain.Entities.Organisations.People.DomainEvents;
using Core.Library.Contracts.DomainEvents.Handle;

namespace App.UseCases.Scheduling.DomainEvents.Organisations.People;

internal sealed class PersonCreatedDomainEventHandler : DomainEventHandler<PersonCreatedDomainEvent>
{
    public override Task Handle(PersonCreatedDomainEvent domainEvent, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}

using Core.Domain.Entities.Organisations.People.DomainEvents;
using Core.Library.Contracts.DomainEvents.Handle;

namespace App.UseCases.Scheduling.DomainEvents.Organisations.People;

internal sealed class PersonEmailUpdatedDomainEventHandler : DomainEventHandler<PersonEmailUpdatedDomainEvent>
{
    public override Task Handle(PersonEmailUpdatedDomainEvent domainEvent, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}

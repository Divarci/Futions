using Core.Domain.Entities.Organisations.People.DomainEvents;
using Core.Library.Contracts.DomainEvents.Handle;

namespace App.UseCases.Scheduling.DomainEvents.Organisations.People;

internal sealed class PersonFullnameUpdatedDomainEventHandler : DomainEventHandler<PersonFullnameUpdatedDomainEvent>
{
    public override Task Handle(PersonFullnameUpdatedDomainEvent domainEvent, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}

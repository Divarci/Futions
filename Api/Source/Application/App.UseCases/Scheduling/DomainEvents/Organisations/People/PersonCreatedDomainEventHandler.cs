using Core.Domain.Entities.Organisations.People.DomainEvents;
using Core.Library.Contracts.DomainEvents.Handle;
using Core.Library.Exceptions;

namespace App.UseCases.Scheduling.DomainEvents.Organisations.People;

internal sealed class PersonCreatedDomainEventHandler : DomainEventHandler<PersonCreatedDomainEvent>
{
    public override Task Handle(PersonCreatedDomainEvent domainEvent, CancellationToken cancellationToken = default)
    {
        throw new FutionsException(
            assemblyName: "App.UseCases",
            className: nameof(PersonCreatedDomainEventHandler),
            methodName: nameof(Handle),
            message: "Handle method is not implemented.");
    }
}

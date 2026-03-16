using Core.Domain.Entities.Organisations.People.DomainEvents;
using Core.Library.Contracts.DomainEvents.Handle;
using Core.Library.Exceptions;

namespace App.UseCases.Scheduling.DomainEvents.Organisations.People;

internal sealed class PersonDeletedDomainEventHandler : DomainEventHandler<PersonDeletedDomainEvent>
{
    public override Task Handle(PersonDeletedDomainEvent domainEvent, CancellationToken cancellationToken = default)
    {
        throw new FutionsException(
            assemblyName: "App.UseCases",
            className: nameof(PersonDeletedDomainEventHandler),
            methodName: nameof(Handle),
            message: "Handle method is not implemented.");
    }
}

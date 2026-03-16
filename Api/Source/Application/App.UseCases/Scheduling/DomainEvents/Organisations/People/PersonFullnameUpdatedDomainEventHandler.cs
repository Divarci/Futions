using Core.Domain.Entities.Organisations.People.DomainEvents;
using Core.Library.Contracts.DomainEvents.Handle;
using Core.Library.Exceptions;

namespace App.UseCases.Scheduling.DomainEvents.Organisations.People;

internal sealed class PersonFullnameUpdatedDomainEventHandler : DomainEventHandler<PersonFullnameUpdatedDomainEvent>
{
    public override Task Handle(PersonFullnameUpdatedDomainEvent domainEvent, CancellationToken cancellationToken = default)
    {
        throw new FutionsException(
            assemblyName: "App.UseCases",
            className: nameof(PersonFullnameUpdatedDomainEventHandler),
            methodName: nameof(Handle),
            message: "Handle method is not implemented.");
    }
}

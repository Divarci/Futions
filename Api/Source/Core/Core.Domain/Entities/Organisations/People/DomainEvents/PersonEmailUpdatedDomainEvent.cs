using Core.Library.Contracts.DomainEvents.Publish;

namespace Core.Domain.Entities.Organisations.People.DomainEvents;

public sealed class PersonEmailUpdatedDomainEvent(
    Guid personId) : DomainEvent
{
    public Guid PersonId { get; } = personId;
}

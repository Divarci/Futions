using Core.Library.Contracts.DomainEvents.Publish;

namespace Core.Domain.Entities.Organisations.People.DomainEvents;

public sealed class PersonDeletedDomainEvent(
    Guid personId) : DomainEvent
{
    public Guid PersonId { get; } = personId;
}

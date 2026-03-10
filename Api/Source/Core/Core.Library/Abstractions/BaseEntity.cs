using Core.Library.Contracts.DomainEvents.Publish;

namespace Core.Library.Abstractions;

/// <summary>
/// Serves as the base class for all domain entities, providing a unique identifier
/// and a built-in mechanism for raising and managing domain events.
/// </summary>
public abstract class BaseEntity
{
    private readonly List<IDomainEvent> _domainEvents = [];

    protected BaseEntity() { }

    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.ToList();
    public Guid Id { get; init; }

    /// <summary>
    /// Clears all domain events that have been raised by this entity.
    /// Typically called after events have been dispatched.
    /// </summary>
    public void ClearDomainEvents() => _domainEvents.Clear();

    /// <summary>
    /// Raises a domain event by adding it to the entity's internal event collection.
    /// </summary>
    public void Raise(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent);
}
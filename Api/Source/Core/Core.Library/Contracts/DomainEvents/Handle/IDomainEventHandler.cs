using Core.Library.Contracts.DomainEvents.Publish;

namespace Core.Library.Contracts.DomainEvents.Handle;

/// <summary>
/// Defines the contract for handling a specific domain event of type <typeparamref name="TDomainEvent"/>.
/// Extends the non-generic <see cref="IDomainEventHandler"/> to provide strongly-typed event handling.
/// </summary>
/// <typeparam name="TDomainEvent">The specific domain event type this handler processes. Contravariant.</typeparam>
public interface IDomainEventHandler<in TDomainEvent> : IDomainEventHandler
    where TDomainEvent : IDomainEvent
{
    /// <summary>
    /// Handles the specified domain event of type <typeparamref name="TDomainEvent"/>.
    /// </summary>
    Task Handle(TDomainEvent domainEvent, CancellationToken cancellationToken = default);
}

/// <summary>
/// Defines the non-generic contract for handling any <see cref="IDomainEvent"/>.
/// Used as a common base for resolving handlers without knowing the specific event type at compile time.
/// </summary>
public interface IDomainEventHandler
{
    /// <summary>
    /// Handles the specified domain event.
    /// </summary>
    Task Handle(IDomainEvent domainEvent, CancellationToken cancellationToken = default);
}
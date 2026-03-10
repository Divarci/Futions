using Core.Library.Contracts.DomainEvents.Publish;

namespace Core.Library.Contracts.DomainEvents.Handle;

/// <summary>
/// Serves as the base class for handling a specific domain event of type <typeparamref name="TDomainEvent"/>.
/// Implements <see cref="IDomainEventHandler{TDomainEvent}"/> and provides a typed dispatch
/// from the non-generic <see cref="IDomainEvent"/> interface.
/// </summary>
/// <typeparam name="TDomainEvent">The specific domain event type this handler processes.</typeparam>
public abstract class DomainEventHandler<TDomainEvent> : IDomainEventHandler<TDomainEvent>
    where TDomainEvent : IDomainEvent
{
    /// <summary>
    /// Handles the specified domain event of type <typeparamref name="TDomainEvent"/>.
    /// </summary>
    public abstract Task Handle(TDomainEvent domainEvent, CancellationToken cancellationToken = default);

    /// <summary>
    /// Dispatches the non-generic <see cref="IDomainEvent"/> to the typed
    /// <see cref="Handle(TDomainEvent, CancellationToken)"/> overload by casting.
    /// </summary>
    public Task Handle(IDomainEvent domainEvent, CancellationToken cancellationToken = default) =>
        Handle((TDomainEvent)domainEvent, cancellationToken);
}
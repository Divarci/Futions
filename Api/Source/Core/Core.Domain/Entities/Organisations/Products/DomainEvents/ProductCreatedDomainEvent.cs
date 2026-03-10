using Core.Library.Contracts.DomainEvents.Publish;

namespace Core.Domain.Entities.Organisations.Products.DomainEvents;

public sealed class ProductCreatedDomainEvent(
    Guid productId) : DomainEvent
{
    public Guid ProductId { get; } = productId;
}

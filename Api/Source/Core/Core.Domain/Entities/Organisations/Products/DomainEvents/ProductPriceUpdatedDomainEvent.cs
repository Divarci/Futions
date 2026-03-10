using Core.Library.Contracts.DomainEvents.Publish;

namespace Core.Domain.Entities.Organisations.Products.DomainEvents;

public sealed class ProductPriceUpdatedDomainEvent(
    Guid productId) : DomainEvent
{
    public Guid ProductId { get; } = productId;
}

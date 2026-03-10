using Core.Library.Contracts.DomainEvents.Publish;

namespace Core.Domain.Entities.Organisations.Companies.DomainEvents;

public sealed class CompanyNameUpdatedDomainEvent(
    Guid companyId) : DomainEvent
{
    public Guid CompanyId { get; } = companyId;
}


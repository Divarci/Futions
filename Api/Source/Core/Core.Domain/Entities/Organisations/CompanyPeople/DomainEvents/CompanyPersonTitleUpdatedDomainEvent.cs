using Core.Library.Contracts.DomainEvents.Publish;

namespace Core.Domain.Entities.Organisations.CompanyPeople.DomainEvents;

public sealed class CompanyPersonTitleUpdatedDomainEvent(
    Guid companyPersonId) : DomainEvent
{
    public Guid CompanyPersonId { get; } = companyPersonId;
}

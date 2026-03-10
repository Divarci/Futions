using Core.Domain.Entities.Organisations.Companies.Models;
using Core.Library.Contracts.DomainEvents.Publish;

namespace Core.Domain.Entities.Organisations.Companies.DomainEvents;

public sealed class CompanyCreatedDomainEvent(
    Guid companyId) : DomainEvent
{
    public Guid CompanyId { get; } = companyId;
}

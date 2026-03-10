using Core.Domain.ValueObjects.AddressValueObject;

namespace Core.Domain.Entities.Organisations.Companies.Models;

public sealed record CompanyCreateModel
{
    public required string Name { get; init; }
    public required AddressModel AddressModel { get; init; }
    public required Guid TenantId { get; init; }
}

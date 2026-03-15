using Core.Domain.ValueObjects.AddressValueObject;

namespace Core.Domain.Entities.Organisations.Companies.Models;

public sealed record CompanyUpdateModel
{
    public required Guid TenantId { get; init; }
    public required Guid CompanyId { get; init; }
    public required string? Name { get; init; }
    public required AddressModel? AddressModel { get; init; }
}
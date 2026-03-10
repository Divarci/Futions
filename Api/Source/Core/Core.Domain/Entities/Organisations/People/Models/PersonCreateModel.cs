using Core.Domain.ValueObjects.FullnameValueObject;

namespace Core.Domain.Entities.Organisations.People.Models;

public sealed record PersonCreateModel
{
    public required Guid TenantId { get; init; }
    public required FullnameModel FullnameModel { get; init; }
    public required string Email { get; init; }
}

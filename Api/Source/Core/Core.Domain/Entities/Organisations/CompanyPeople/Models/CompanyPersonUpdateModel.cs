namespace Core.Domain.Entities.Organisations.CompanyPeople.Models;

public sealed record CompanyPersonUpdateModel
{
    public required Guid TenantId { get; init; }
    public required Guid CompanyId { get; init; }
    public required Guid CompanyPersonId { get; init; }
    public required string? Title { get; init; }
}

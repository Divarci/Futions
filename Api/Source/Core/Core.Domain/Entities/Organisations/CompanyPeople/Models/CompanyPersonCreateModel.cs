namespace Core.Domain.Entities.Organisations.CompanyPeople.Models;

public sealed record CompanyPersonCreateModel
{
    public required Guid TenantId { get; init; }
    public required Guid CompanyId { get; init; }
    public required Guid PersonId { get; init; }
    public required string Title { get; init; }
}

namespace Core.Domain.Entities.Organisations.Products.Models;

public sealed record ProductCreateModel
{
    public required Guid TenantId { get; init; }
    public required string Name { get; init; }
    public required decimal Price { get; init; }
    public required Guid CompanyId { get; init; }
}

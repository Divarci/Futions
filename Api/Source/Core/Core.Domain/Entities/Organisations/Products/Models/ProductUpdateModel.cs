namespace Core.Domain.Entities.Organisations.Products.Models;

public sealed record ProductUpdateModel
{
    public required Guid ProductId { get; init; }
    public required string? Name { get; init; }
    public required decimal? Price { get; init; }
}

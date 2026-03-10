namespace Core.Domain.Entities.Organisations.CompanyPeople.Models;

public sealed record CompanyPersonUpdateModel
{
    public required string? Title { get; init; }
}

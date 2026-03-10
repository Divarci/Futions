namespace Core.Domain.ValueObjects.FullnameValueObject;

public sealed record FullnameModel
{
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
}

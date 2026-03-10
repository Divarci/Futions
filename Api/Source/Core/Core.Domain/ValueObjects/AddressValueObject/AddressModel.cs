namespace Core.Domain.ValueObjects.AddressValueObject;

public sealed record AddressModel
{
    public required string LineOne { get; init; }
    public required string? LineTwo { get; init; }
    public required string? LineThree { get; init; }
    public required string? LineFour { get; init; }
    public required string Postcode { get; init; }
}

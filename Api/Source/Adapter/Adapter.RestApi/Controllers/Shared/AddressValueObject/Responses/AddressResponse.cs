using System.Text.Json.Serialization;

namespace Adapter.RestApi.Controllers.Shared.AddressValueObject.Responses;

public sealed record AddressResponse
{
    [JsonPropertyName("lineOne")]
    public required string LineOne { get; init; }

    [JsonPropertyName("lineTwo")]
    public required string? LineTwo { get; init; }

    [JsonPropertyName("lineThree")]
    public required string? LineThree { get; init; }

    [JsonPropertyName("lineFour")]
    public required string? LineFour { get; init; }

    [JsonPropertyName("postcode")]
    public required string Postcode { get; init; }

}

using System.Text.Json.Serialization;

namespace Adapter.RestApi.Controllers.Shared.AddressValueObject.Requests;

public sealed record UpdateAddressRequest
{
    [JsonPropertyName("addressLineOne")]
    public string? AddressLineOne { get; init; }

    [JsonPropertyName("addressLineTwo")]
    public string? AddressLineTwo { get; init; }

    [JsonPropertyName("addressLineThree")]
    public string? AddressLineThree { get; init; }

    [JsonPropertyName("addressLineFour")]
    public string? AddressLineFour { get; init; }

    [JsonPropertyName("postcode")]
    public string? Postcode { get; init; }
}
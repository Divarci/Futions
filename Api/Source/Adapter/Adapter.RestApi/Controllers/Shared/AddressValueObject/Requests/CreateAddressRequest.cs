using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Adapter.RestApi.Controllers.Shared.AddressValueObject.Requests;

public sealed record CreateAddressRequest
{
    [Required]
    [JsonPropertyName("addressLineOne")]
    public string? AddressLineOne { get; init; } = default!;

    [JsonPropertyName("addressLineTwo")]
    public string? AddressLineTwo { get; init; }

    [JsonPropertyName("addressLineThree")]
    public string? AddressLineThree { get; init; }

    [JsonPropertyName("addressLineFour")]
    public string? AddressLineFour { get; init; }

    [Required]
    [JsonPropertyName("postcode")]
    public string? Postcode { get; init; } = default!;
}
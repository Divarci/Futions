using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Adapter.RestApi.Controllers.Shared.AddressValueObject.Requests;

public sealed record UpdateAddressRequest
{
    [Required, MaxLength(100)]
    [JsonPropertyName("addressLineOne")]
    public string? AddressLineOne { get; init; }

    [MaxLength(100)]
    [JsonPropertyName("addressLineTwo")]
    public string? AddressLineTwo { get; init; }

    [MaxLength(100)]
    [JsonPropertyName("addressLineThree")]
    public string? AddressLineThree { get; init; }

    [MaxLength(100)]
    [JsonPropertyName("addressLineFour")]
    public string? AddressLineFour { get; init; }

    [Required, MaxLength(20)]
    [JsonPropertyName("postcode")]
    public string? Postcode { get; init; }
}
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Adapter.RestApi.Controllers.Shared.FullnameValueObject.Requests;

public sealed record UpdateFullnameRequest
{
    [Required, MaxLength(50)]
    [JsonPropertyName("firstName")]
    public string? FirstName { get; init; }

    [Required, MaxLength(50)]
    [JsonPropertyName("lastName")]
    public string? LastName { get; init; }
}

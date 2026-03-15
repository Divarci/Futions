using System.Text.Json.Serialization;

namespace Adapter.RestApi.Controllers.Shared.FullnameValueObject.Responses;

public record FullnameResponse
{
    [JsonPropertyName("firstName")]
    public required string FirstName { get; init; }

    [JsonPropertyName("lastName")]
    public required string LastName { get; init; }
}

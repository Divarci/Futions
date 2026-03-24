using Adapter.RestApi.Controllers.Shared.FullnameValueObject.Responses;
using System.Text.Json.Serialization;

namespace Adapter.RestApi.Controllers.VersionOne.Organisations.People.Models.Responses;

public sealed record PersonResponse
{
    [JsonPropertyName("id")]
    public required Guid Id { get; init; }

    [JsonPropertyName("fullname")]
    public required FullnameResponse Fullname { get; init; }

    [JsonPropertyName("email")]
    public required string? Email { get; init; }
}

using Adapter.RestApi.Controllers.Shared.AddressValueObject.Responses;
using System.Text.Json.Serialization;

namespace Adapter.RestApi.Controllers.VersionOne.Organisations.Companies.Core.Models.Responses;

public sealed record CompanyResponse
{
    [JsonPropertyName("id")]
    public required Guid Id { get; init; }

    [JsonPropertyName("name")]
    public required string Name { get; init; }

    [JsonPropertyName("address")]
    public required AddressResponse? Address { get; init; }
}

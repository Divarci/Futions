using System.Text.Json.Serialization;

namespace Adapter.RestApi.Controllers.VersionOne.Organisations.Companies.CompanyProducts.Models.Responses;

public record CompanyProductResponse
{
    [JsonPropertyName("id")]
    public required Guid Id { get; init; }

    [JsonPropertyName("companyId")]
    public required Guid CompanyId { get; init; }

    [JsonPropertyName("name")]
    public required string Name { get; init; }

    [JsonPropertyName("price")]
    public required decimal Price { get; init; }
}

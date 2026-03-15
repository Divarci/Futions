using System.Text.Json.Serialization;

namespace Adapter.RestApi.Controllers.VersionOne.Organisations.Companies.CompanyPeople.Models.Responses;

public record CompanyPersonResponse
{
    [JsonPropertyName("id")]
    public required Guid Id { get; init; }

    [JsonPropertyName("companyId")]
    public required Guid CompanyId { get; init; }

    [JsonPropertyName("personId")]
    public required Guid PersonId { get; init; }

    [JsonPropertyName("title")]
    public required string Title { get; init; }
}

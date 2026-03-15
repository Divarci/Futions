using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Adapter.RestApi.Controllers.VersionOne.Organisations.Companies.Models.Requests;

public record CreateCompanyRequest
{
    [Required, JsonPropertyName("name")]
    [MaxLength(100)]
    public string? Name { get; init; }
}

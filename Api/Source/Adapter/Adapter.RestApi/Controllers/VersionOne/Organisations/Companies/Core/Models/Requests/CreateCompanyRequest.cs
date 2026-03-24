using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Adapter.RestApi.Controllers.VersionOne.Organisations.Companies.Core.Models.Requests;

public sealed record CreateCompanyRequest
{
    [Required, JsonPropertyName("name")]
    [MaxLength(100)]
    public string? Name { get; init; }
}

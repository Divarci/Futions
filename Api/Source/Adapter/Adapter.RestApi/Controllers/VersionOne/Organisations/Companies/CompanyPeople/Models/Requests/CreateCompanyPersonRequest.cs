using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Adapter.RestApi.Controllers.VersionOne.Organisations.Companies.CompanyPeople.Models.Requests;

public record CreateCompanyPersonRequest
{
    [Required, JsonPropertyName("personId")]
    public Guid? PersonId { get; init; }

    [Required, JsonPropertyName("title")]
    [MaxLength(100)]
    public string? Title { get; init; }
}

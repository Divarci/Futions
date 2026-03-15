using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Adapter.RestApi.Controllers.VersionOne.Organisations.Companies.CompanyProducts.Models.Requests;

public record CreateCompanyProductRequest
{
    [Required, JsonPropertyName("name")]
    [MaxLength(100)]
    public string? Name { get; init; }

    [Required, JsonPropertyName("price")]
    [Range(0, double.MaxValue)]
    public decimal? Price { get; init; }
}

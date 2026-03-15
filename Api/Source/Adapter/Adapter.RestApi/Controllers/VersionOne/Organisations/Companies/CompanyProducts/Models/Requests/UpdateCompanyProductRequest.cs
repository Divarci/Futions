using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Adapter.RestApi.Controllers.VersionOne.Organisations.Companies.CompanyProducts.Models.Requests;

public class UpdateCompanyProductRequest
{
    [JsonPropertyName("name")]
    [MaxLength(100)]
    public string? Name { get; init; }

    [JsonPropertyName("price")]
    [Range(0, double.MaxValue)]
    public decimal? Price { get; init; }
}

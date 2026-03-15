using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Adapter.RestApi.Controllers.VersionOne.Organisations.Products.Models.Requests;

public record CreateProductRequest
{
    [Required, JsonPropertyName("companyId")]
    public Guid? CompanyId { get; init; }

    [Required, JsonPropertyName("name")]
    [MaxLength(100)]
    public string? Name { get; init; }

    [Required, JsonPropertyName("price")]
    [Range(0, double.MaxValue)]
    public decimal? Price { get; init; }
}

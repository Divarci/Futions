using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Adapter.RestApi.Controllers.VersionOne.Organisations.Products.Models.Requests;

public class UpdateProductRequest
{
    [JsonPropertyName("name")]
    [MaxLength(100)]
    public string? Name { get; init; }

    [JsonPropertyName("price")]
    [Range(0, double.MaxValue)]
    public decimal? Price { get; init; }
}

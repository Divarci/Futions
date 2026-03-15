using Adapter.RestApi.Controllers.Shared.AddressValueObject.Requests;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Adapter.RestApi.Controllers.VersionOne.Organisations.Companies.Core.Models.Requests;

public class UpdateCompanyRequest
{
    [JsonPropertyName("name")]
    [MaxLength(100)]
    public string? Name { get; init; }

    [JsonPropertyName("address")]
    public UpdateAddressRequest? Address { get; init; }
}

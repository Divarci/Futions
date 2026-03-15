using Adapter.RestApi.Controllers.Shared.FullnameValueObject.Requests;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Adapter.RestApi.Controllers.VersionOne.Organisations.People.Models.Requests;

public class UpdatePersonRequest
{
    [JsonPropertyName("fullname")]
    public UpdateFullnameRequest? Fullname { get; init; }

    [JsonPropertyName("email")]
    [MaxLength(200)]
    public string? Email { get; init; }
}

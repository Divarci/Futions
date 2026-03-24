using Adapter.RestApi.Controllers.Shared.FullnameValueObject.Requests;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Adapter.RestApi.Controllers.VersionOne.Organisations.People.Models.Requests;

public sealed record UpdatePersonRequest
{
    [JsonPropertyName("fullname")]
    public UpdateFullnameRequest? Fullname { get; init; }

    [JsonPropertyName("email")]
    [MaxLength(100), EmailAddress]
    public string? Email { get; init; }
}

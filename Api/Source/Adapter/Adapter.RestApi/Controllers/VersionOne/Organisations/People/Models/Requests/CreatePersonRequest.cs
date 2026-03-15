using Adapter.RestApi.Controllers.Shared.FullnameValueObject.Requests;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Adapter.RestApi.Controllers.VersionOne.Organisations.People.Models.Requests;

public record CreatePersonRequest
{
    [Required, JsonPropertyName("fullname")]
    public UpdateFullnameRequest? Fullname { get; init; }
}

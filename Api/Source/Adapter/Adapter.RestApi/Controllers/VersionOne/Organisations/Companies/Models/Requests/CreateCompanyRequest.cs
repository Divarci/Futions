using Adapter.RestApi.Controllers.Shared.AddressValueObject.Requests;
using System.ComponentModel.DataAnnotations;

namespace Adapter.RestApi.Controllers.VersionOne.Organisations.Companies.Models.Requests;

public record CreateCompanyRequest
{
    [Required]
    public string? Name { get; init; } = default!;

    [Required]
    public CreateAddressRequest? Address { get; init; } = default!;
}

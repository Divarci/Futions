using System.ComponentModel.DataAnnotations;

namespace Adapter.RestApi.Controllers.VersionOne.Organisations.Companies.Models.Requests;

public record CreateCompanyRequest
{
    [Required]
    public string? Name { get; init; }

}

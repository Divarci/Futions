using Adapter.RestApi.Controllers.Shared.AddressValueObject.Requests;

namespace Adapter.RestApi.Controllers.VersionOne.Organisations.Companies.Models.Requests;

public class UpdateCompanyRequest
{
    public string? Name { get; init; }

    public CreateAddressRequest? Address { get; init; }
}

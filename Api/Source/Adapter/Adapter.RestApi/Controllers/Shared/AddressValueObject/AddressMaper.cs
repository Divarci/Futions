using Adapter.RestApi.Controllers.Shared.AddressValueObject.Requests;
using Adapter.RestApi.Controllers.Shared.AddressValueObject.Responses;
using Core.Domain.ValueObjects.AddressValueObject;

namespace Adapter.RestApi.Controllers.Shared.AddressValueObject;

internal static class AddressMaper
{
    internal static AddressModel ToCreateModel(CreateAddressRequest request)
        => new()
        {
            LineOne = request.AddressLineOne!,
            LineTwo = request.AddressLineTwo,
            LineThree = request.AddressLineThree,
            LineFour = request.AddressLineFour,
            Postcode = request.Postcode!
        };

    internal static AddressModel ToUpdateModel(CreateAddressRequest request)
       => new()
       {
           LineOne = request.AddressLineOne!,
           LineTwo = request.AddressLineTwo,
           LineThree = request.AddressLineThree,
           LineFour = request.AddressLineFour,
           Postcode = request.Postcode!
       };

    internal static AddressResponse ToResponse(Address address)
        => new()
        {
            LineOne = address.LineOne,
            LineTwo = address.LineTwo,
            LineThree = address.LineThree,
            LineFour = address.LineFour,
            Postcode = address.Postcode
        };
}

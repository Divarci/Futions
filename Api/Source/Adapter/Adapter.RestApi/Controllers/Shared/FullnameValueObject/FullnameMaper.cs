using Adapter.RestApi.Controllers.Shared.FullnameValueObject.Requests;
using Adapter.RestApi.Controllers.Shared.FullnameValueObject.Responses;
using Core.Domain.ValueObjects.FullnameValueObject;

namespace Adapter.RestApi.Controllers.Shared.FullnameValueObject;

internal static class FullnameMaper
{
    internal static FullnameModel ToUpdateModel(UpdateFullnameRequest request)
        => new()
        {
            FirstName = request.FirstName!,
            LastName = request.LastName!
        };

    internal static FullnameResponse ToResponse(Fullname fullname)
        => new()
        {
            FirstName = fullname.FirstName,
            LastName = fullname.LastName
        };
}

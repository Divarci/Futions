using Adapter.RestApi.Controllers.Shared.FullnameValueObject;
using Adapter.RestApi.Controllers.VersionOne.Organisations.People.Models.Requests;
using Adapter.RestApi.Controllers.VersionOne.Organisations.People.Models.Responses;
using Core.Domain.Entities.Organisations.People;
using Core.Domain.Entities.Organisations.People.Models;

namespace Adapter.RestApi.Controllers.VersionOne.Organisations.People;

internal static class PersonMapper
{
    internal static PersonResponse ToResponse(Person person)
        => new()
        {
            Id = person.Id,
            Fullname = FullnameMaper.ToResponse(person.Fullname),
            Email = person.Email
        };

    internal static PersonResponse[] ToArrayResponse(Person[] people)
        => [.. people.Select(ToResponse)];

    internal static PersonCreateModel ToCreateModel(CreatePersonRequest request, Guid tenantId)
        => new()
        {
            TenantId = tenantId,
            FullnameModel = FullnameMaper.ToUpdateModel(request.Fullname!)
        };

    internal static PersonUpdateModel ToUpdateModel(UpdatePersonRequest request, Guid tenantId, Guid personId)
        => new()
        {
            TenantId = tenantId,
            PersonId = personId,
            FullnameModel = request.Fullname is not null
                ? FullnameMaper.ToUpdateModel(request.Fullname)
                : null,
            Email = request.Email
        };
}

using Adapter.RestApi.Controllers.VersionOne.Organisations.Companies.CompanyPeople.Models.Requests;
using Adapter.RestApi.Controllers.VersionOne.Organisations.Companies.CompanyPeople.Models.Responses;
using Core.Domain.Entities.Organisations.CompanyPeople;
using Core.Domain.Entities.Organisations.CompanyPeople.Models;

namespace Adapter.RestApi.Controllers.VersionOne.Organisations.Companies.CompanyPeople;

internal static class CompanyPersonMapper
{
    internal static CompanyPersonResponse ToResponse(CompanyPerson companyPerson)
        => new()
        {
            Id = companyPerson.Id,
            CompanyId = companyPerson.CompanyId,
            PersonId = companyPerson.PersonId,
            Title = companyPerson.Title
        };

    internal static CompanyPersonResponse[] ToArrayResponse(CompanyPerson[] companyPeople)
        => [.. companyPeople.Select(ToResponse)];

    internal static CompanyPersonCreateModel ToCreateModel(CreateCompanyPersonRequest request, Guid companyId)
        => new()
        {
            CompanyId = companyId,
            PersonId = request.PersonId!.Value,
            Title = request.Title!
        };

    internal static CompanyPersonUpdateModel ToUpdateModel(
        UpdateCompanyPersonRequest request,Guid tenantId, Guid companyId, Guid companyPersonId)
        => new()
        {
            TenantId = tenantId,
            CompanyId = companyId,
            CompanyPersonId = companyPersonId,
            Title = request.Title
        };
}

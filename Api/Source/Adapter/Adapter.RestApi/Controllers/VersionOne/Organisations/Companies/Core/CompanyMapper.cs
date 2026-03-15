using Adapter.RestApi.Controllers.Shared.AddressValueObject;
using Adapter.RestApi.Controllers.VersionOne.Organisations.Companies.Core.Models.Requests;
using Adapter.RestApi.Controllers.VersionOne.Organisations.Companies.Core.Models.Responses;
using Core.Domain.Entities.Organisations.Companies;
using Core.Domain.Entities.Organisations.Companies.Models;

namespace Adapter.RestApi.Controllers.VersionOne.Organisations.Companies.Core;

internal static class CompanyMapper
{
    internal static CompanyResponse ToResponse(Company company)
        => new()
        {
            Id = company.Id,
            Name = company.Name,
            Address = company.Address is not null
                ? AddressMaper.ToResponse(company.Address)
                : null
        };

    internal static CompanyResponse[] ToArrayResponse(Company[] companies)
        => [.. companies.Select(ToResponse)];

    internal static CompanyCreateModel ToCreateModel(CreateCompanyRequest request, Guid tenantId)
        => new()
        {
            TenantId = tenantId,
            Name = request.Name!
        };

    internal static CompanyUpdateModel ToUpdateModel(UpdateCompanyRequest request,Guid tenantId, Guid companyId)
        => new()
        {
            TenantId = tenantId,
            CompanyId = companyId,
            Name = request.Name,
            AddressModel = request.Address is not null
                ? AddressMaper.ToUpdateModel(request.Address)
                : null
        };
}

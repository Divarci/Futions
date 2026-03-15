using Adapter.RestApi.Controllers.Shared.AddressValueObject;
using Adapter.RestApi.Controllers.VersionOne.Organisations.Companies.Models.Requests;
using Adapter.RestApi.Controllers.VersionOne.Organisations.Companies.Models.Responses;
using Core.Domain.Entities.Organisations.Companies;
using Core.Domain.Entities.Organisations.Companies.Models;

namespace Adapter.RestApi.Controllers.VersionOne.Organisations.Companies;

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
            Name = request.Name!,
            AddressModel = AddressMaper.ToCreateModel(request.Address!)
        };

    internal static CompanyUpdateModel ToUpdateModel(UpdateCompanyRequest request, Guid companyId)
        => new()
        {
            CompanyId = companyId,
            Name = request.Name,
            AddressModel = AddressMaper.ToUpdateModel(request.Address!)
        };
}

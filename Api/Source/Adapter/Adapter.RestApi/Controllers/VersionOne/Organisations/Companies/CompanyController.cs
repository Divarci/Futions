using Adapter.RestApi.AspNetCore.Extensions;
using Adapter.RestApi.Controllers.Shared.Models;
using Adapter.RestApi.Controllers.VersionOne.Organisations.Companies.Models.Requests;
using Adapter.RestApi.Controllers.VersionOne.System.AuditLogs;
using Asp.Versioning;
using Core.Domain.Entities.Organisations.Companies;
using Core.Domain.Entities.Organisations.Companies.Interfaces;
using Core.Domain.Entities.Organisations.Companies.Models;
using Core.Domain.ValueObjects.AuditStampValueObject;
using Core.Library.ResultPattern;
using Microsoft.AspNetCore.Mvc;

namespace Adapter.RestApi.Controllers.VersionOne.Organisations.Companies;

[ApiVersion(ApiVersion.V1)]
[Route("api/v{version:apiVersion}/tenants/{tenantId:guid}/companies")]
[ApiController]
public class CompanyController(
    ICompanyUseCase companyUseCase) : BaseController
{
    private readonly ICompanyUseCase _companyUseCase = companyUseCase;

    [HttpGet]
    public async Task<IActionResult> GetCompaniesAsync(
        Guid tenantId,
        [FromQuery] PaginationFilterModel filter,
        CancellationToken cancellationToken)
    {
        PaginatedResult<Company[]> paginatedCompanies = await _companyUseCase.GetPaginatedAsync(
            tenantId,
            filter.Page,
            filter.PageSize,
            filter.SortBy,
            filter.IsAscending,
            filter.Filter,
            cancellationToken);

        return HandleResult(
            result: paginatedCompanies,
            mapper: CompanyMapper.ToArrayResponse);
    }

    [HttpGet("{companyId}")]
    public async Task<IActionResult> GetCompanyAsync(
        Guid tenantId,
        Guid companyId,
        CancellationToken cancellationToken)
    {
        Result<Company> company = await _companyUseCase.GetByIdAsync(
            tenantId,
            companyId,
            cancellationToken);

        return HandleResult(
            result: company,
            mapper: CompanyMapper.ToResponse);
    }

    [HttpPost]
    public async Task<IActionResult> CreateCompanyAsync(
        Guid tenantId,
        CreateCompanyRequest request,
        CancellationToken cancellationToken = default)
    {
        CompanyCreateModel companyCreateModel = CompanyMapper.ToCreateModel(request, tenantId);
        AuditStampCreateModel auditLogCreateModel = AuditLogMapper.ToCreateModel(
            HttpContext.GetRequiredUserId(),
            HttpContext.GetRequiredUserEmail(),
            tenantId);

        Result<Company> createdCompany = await _companyUseCase.CreateAsync(
            companyCreateModel,
            auditLogCreateModel,
            cancellationToken);

        return HandleResult(
            result: createdCompany,
            mapper: CompanyMapper.ToResponse);
    }

    [HttpPatch("{companyId}")]
    public async Task<IActionResult> UpdateCompanyAsync(
        Guid tenantId,
        Guid companyId,
        UpdateCompanyRequest request,
        CancellationToken cancellationToken = default)
    {
        CompanyUpdateModel companyUpdateModel = CompanyMapper.ToUpdateModel(request, companyId);
        AuditStampCreateModel auditStampCreateModel = AuditLogMapper.ToCreateModel(
            HttpContext.GetRequiredUserId(),
            HttpContext.GetRequiredUserEmail(),
            tenantId);

        Result updatedCompany = await _companyUseCase.UpdateAsync(
            tenantId,
            companyUpdateModel,
            auditStampCreateModel,
            cancellationToken);

        return HandleResult(result: updatedCompany);
    }

    [HttpDelete("{companyId}")]
    public async Task<IActionResult> DeleteCompanyAsync(
        Guid tenantId,
        Guid companyId,
        CancellationToken cancellationToken = default)
    {
        AuditStampCreateModel auditStampCreateModel = AuditLogMapper.ToCreateModel(
            HttpContext.GetRequiredUserId(),
            HttpContext.GetRequiredUserEmail(),
            tenantId);

        Result deletedCompany = await _companyUseCase.DeleteAsync(
            tenantId,
            companyId,
            auditStampCreateModel,
            cancellationToken);

        return HandleResult(result: deletedCompany);
    }
}
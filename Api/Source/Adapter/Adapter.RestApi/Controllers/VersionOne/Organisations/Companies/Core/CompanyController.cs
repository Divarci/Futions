using Adapter.RestApi.AspNetCore.Authentication;
using Adapter.RestApi.AspNetCore.Filters;
using Adapter.RestApi.Controllers.Shared.Mappers;
using Adapter.RestApi.Controllers.Shared.Models;
using Adapter.RestApi.Controllers.VersionOne.Organisations.Companies.Core.Models.Requests;
using Adapter.RestApi.Controllers.VersionOne.Organisations.Companies.Core.Models.Responses;
using Asp.Versioning;
using Core.Domain.Entities.Organisations.Companies;
using Core.Domain.Entities.Organisations.Companies.Interfaces;
using Core.Domain.Entities.Organisations.Companies.Models;
using Core.Domain.ValueObjects.AuditStampValueObject;
using Core.Library.ResultPattern;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Adapter.RestApi.Controllers.VersionOne.Organisations.Companies.Core;

[ApiVersion(ApiVersion.V1)]
[Route("api/v{version:apiVersion}/tenants/{tenantId:guid}/companies")]
[ApiController]
[Authorize(Policy = PolicyNames.AllRoles)]
[TenantAuthorization]
public class CompanyController(
    ICompanyUseCase companyUseCase) : BaseController
{
    private readonly ICompanyUseCase _companyUseCase = companyUseCase;

    [HttpGet]
    [ProducesResponseType<PaginatedResult<CompanyResponse[]>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status403Forbidden)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetCompaniesAsync(
        Guid tenantId,
        [FromQuery] PaginationFilterModel filter,
        CancellationToken cancellationToken)
    {
        PaginatedResult<CompanyResponse[]> paginatedCompanies = await _companyUseCase.GetPaginatedCompaniesAsync(
            tenantId,
            filter.Page,
            filter.PageSize,
            filter.SortBy,
            filter.IsAscending,
            filter.Filter,
            CompanyMapper.ToArrayResponse,
            cancellationToken);

        return HandleResult(paginatedCompanies);
    }

    [HttpGet("{companyId}")]
    [ProducesResponseType<PaginatedResult<CompanyResponse[]>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status403Forbidden)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetCompanyAsync(
        Guid tenantId,
        Guid companyId,
        CancellationToken cancellationToken)
    {
        Result<CompanyResponse> company = await _companyUseCase.GetCompanyByIdAsync(
            tenantId,
            companyId,
            CompanyMapper.ToResponse,
            cancellationToken);

        return HandleResult(company);
    }

    [Authorize(Policy = PolicyNames.AdminOrSystemAdmin)]
    [HttpPost]
    [ProducesResponseType<Result<CompanyResponse>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status403Forbidden)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateCompanyAsync(
        Guid tenantId,
        [FromBody] CreateCompanyRequest request,
        CancellationToken cancellationToken = default)
    {
        CompanyCreateModel companyCreateModel = CompanyMapper.ToCreateModel(request, tenantId);
        AuditStampCreateModel auditLogCreateModel = AuditLogMapper.ToCreateModel(
            GetCurrentUserId(),
            GetCurrentUsername(),
            tenantId);

        Result<Company> createdCompany = await _companyUseCase.CreateCompanyAsync(
            companyCreateModel,
            auditLogCreateModel,
            cancellationToken);

        return HandleResult(
            result: createdCompany,
            mapper: CompanyMapper.ToResponse);
    }

    [Authorize(Policy = PolicyNames.AdminOrSystemAdmin)]
    [HttpPatch("{companyId}")]
    [ProducesResponseType<Result<CompanyResponse>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status403Forbidden)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateCompanyAsync(
        Guid tenantId,
        Guid companyId,
        [FromBody] UpdateCompanyRequest request,
        CancellationToken cancellationToken = default)
    {
        CompanyUpdateModel companyUpdateModel = CompanyMapper.ToUpdateModel(request, tenantId, companyId);
        AuditStampCreateModel auditStampCreateModel = AuditLogMapper.ToCreateModel(
            GetCurrentUserId(),
            GetCurrentUsername(),
            tenantId);

        Result updatedCompany = await _companyUseCase.UpdateCompanyAsync(
            companyUpdateModel,
            auditStampCreateModel,
            cancellationToken);

        return HandleResult(result: updatedCompany);
    }

    [Authorize(Policy = PolicyNames.AdminOrSystemAdmin)]
    [HttpDelete("{companyId}")]
    [ProducesResponseType<Result<CompanyResponse>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status403Forbidden)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteCompanyAsync(
        Guid tenantId,
        Guid companyId,
        CancellationToken cancellationToken = default)
    {
        AuditStampCreateModel auditStampCreateModel = AuditLogMapper.ToCreateModel(
            GetCurrentUserId(),
            GetCurrentUsername(),
            tenantId);

        Result deletedCompany = await _companyUseCase.DeleteCompanyAsync(
            tenantId,
            companyId,
            auditStampCreateModel,
            cancellationToken);

        return HandleResult(result: deletedCompany);
    }
}

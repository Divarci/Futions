using Adapter.RestApi.AspNetCore.Authentication;
using Adapter.RestApi.AspNetCore.Filters;
using Adapter.RestApi.Controllers.Shared.Mappers;
using Adapter.RestApi.Controllers.Shared.Models;
using Adapter.RestApi.Controllers.VersionOne.Organisations.Companies.CompanyPeople.Models.Requests;
using Adapter.RestApi.Controllers.VersionOne.Organisations.Companies.CompanyPeople.Models.Responses;
using Asp.Versioning;
using Core.Domain.Entities.Organisations.CompanyPeople;
using Core.Domain.Entities.Organisations.CompanyPeople.Interfaces;
using Core.Domain.Entities.Organisations.CompanyPeople.Models;
using Core.Domain.ValueObjects.AuditStampValueObject;
using Core.Library.ResultPattern;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Adapter.RestApi.Controllers.VersionOne.Organisations.Companies.CompanyPeople;

[ApiVersion(ApiVersion.V1)]
[Route("api/v{version:apiVersion}/tenants/{tenantId:guid}/companies/{companyId:guid}/company-people")]
[ApiController]
[Authorize(Policy = PolicyNames.AllRoles)]
[TenantAuthorization]
public class CompanyPersonController(
    ICompanyPersonUseCase companyPersonUseCase) : BaseController
{
    private readonly ICompanyPersonUseCase _companyPersonUseCase = companyPersonUseCase;

    [HttpGet]
    [ProducesResponseType<PaginatedResult<CompanyPersonResponse[]>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status403Forbidden)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetCompanyPeopleAsync(
        Guid tenantId,
        Guid companyId,
        [FromQuery] PaginationFilterModel filter,
        CancellationToken cancellationToken)
    {
        PaginatedResult<CompanyPersonResponse[]> paginatedCompanyPeople = await _companyPersonUseCase.GetPaginatedCompanyPeopleAsync(
            tenantId,
            companyId,
            filter.Page,
            filter.PageSize,
            filter.SortBy,
            filter.IsAscending,
            filter.Filter,
            CompanyPersonMapper.ToArrayResponse,
            cancellationToken);

        return HandleResult(paginatedCompanyPeople);
    }

    [HttpGet("{companyPersonId}")]
    [ProducesResponseType<PaginatedResult<CompanyPersonResponse[]>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status403Forbidden)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetCompanyPersonAsync(
        Guid tenantId,
        Guid companyId,
        Guid companyPersonId,
        CancellationToken cancellationToken)
    {
        Result<CompanyPersonResponse> companyPerson = await _companyPersonUseCase.GetCompanyPersonByIdAsync(
            tenantId,
            companyId,
            companyPersonId,
            CompanyPersonMapper.ToResponse,
            cancellationToken);

        return HandleResult(companyPerson);
    }

    [Authorize(Policy = PolicyNames.AdminOrSystemAdmin)]
    [HttpPost]
    [ProducesResponseType<Result<CompanyPersonResponse>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status403Forbidden)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateCompanyPersonAsync(  
        Guid tenantId,
        Guid companyId,
        [FromBody] CreateCompanyPersonRequest request,
        CancellationToken cancellationToken = default)
    {
        CompanyPersonCreateModel companyPersonCreateModel = CompanyPersonMapper.ToCreateModel(request, companyId);
        AuditStampCreateModel auditLogCreateModel = AuditLogMapper.ToCreateModel(
            GetCurrentUserId(),
            GetCurrentUsername(),
            tenantId);

        Result<CompanyPerson> createdCompanyPerson = await _companyPersonUseCase.CreateCompanyPersonAsync(
            companyPersonCreateModel,
            auditLogCreateModel,
            cancellationToken);

        return HandleResult(
            result: createdCompanyPerson,
            mapper: CompanyPersonMapper.ToResponse);
    }

    [Authorize(Policy = PolicyNames.AdminOrSystemAdmin)]
    [HttpPatch("{companyPersonId}")]
    [ProducesResponseType<Result<CompanyPersonResponse>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status403Forbidden)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateCompanyPersonAsync(
        Guid tenantId,
        Guid companyId,
        Guid companyPersonId,
        [FromBody] UpdateCompanyPersonRequest request,
        CancellationToken cancellationToken = default)
    {
        CompanyPersonUpdateModel companyPersonUpdateModel = CompanyPersonMapper.ToUpdateModel(
            request, tenantId, companyId, companyPersonId);

        AuditStampCreateModel auditStampCreateModel = AuditLogMapper.ToCreateModel(
            GetCurrentUserId(),
            GetCurrentUsername(),
            tenantId);

        Result updatedCompanyPerson = await _companyPersonUseCase.UpdateCompanyPersonAsync(
            companyPersonUpdateModel,
            auditStampCreateModel,
            cancellationToken);

        return HandleResult(result: updatedCompanyPerson);
    }

    [Authorize(Policy = PolicyNames.AdminOrSystemAdmin)]
    [HttpDelete("{companyPersonId}")]
    [ProducesResponseType<Result<CompanyPersonResponse>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status403Forbidden)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteCompanyPersonAsync(
        Guid tenantId,
        Guid companyId,
        Guid companyPersonId,
        CancellationToken cancellationToken = default)
    {
        AuditStampCreateModel auditStampCreateModel = AuditLogMapper.ToCreateModel(
            GetCurrentUserId(),
            GetCurrentUsername(),
            tenantId);

        Result deletedCompanyPerson = await _companyPersonUseCase.DeleteCompanyPersonAsync(
            tenantId,
            companyId,
            companyPersonId,
            auditStampCreateModel,
            cancellationToken);

        return HandleResult(result: deletedCompanyPerson);
    }
}

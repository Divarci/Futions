using Adapter.RestApi.AspNetCore.Extensions;
using Adapter.RestApi.Controllers.Shared.Models;
using Adapter.RestApi.Controllers.VersionOne.Organisations.People.Models.Requests;
using Adapter.RestApi.Controllers.VersionOne.Organisations.People.Models.Responses;
using Adapter.RestApi.Controllers.VersionOne.System.AuditLogs;
using Asp.Versioning;
using Core.Domain.Entities.Organisations.People;
using Core.Domain.Entities.Organisations.People.Interfaces;
using Core.Domain.Entities.Organisations.People.Models;
using Core.Domain.ValueObjects.AuditStampValueObject;
using Core.Library.ResultPattern;
using Microsoft.AspNetCore.Mvc;

namespace Adapter.RestApi.Controllers.VersionOne.Organisations.People;

[ApiVersion(ApiVersion.V1)]
[Route("api/v{version:apiVersion}/tenants/{tenantId:guid}/people")]
[ApiController]
public class PersonController(
    IPersonUseCase personUseCase) : BaseController
{
    private readonly IPersonUseCase _personUseCase = personUseCase;

    [HttpGet]
    public async Task<IActionResult> GetPeopleAsync(
        Guid tenantId,
        [FromQuery] PaginationFilterModel filter,
        CancellationToken cancellationToken)
    {
        PaginatedResult<PersonResponse[]> paginatedPeople = await _personUseCase.GetPaginatedAsync(
            tenantId,
            filter.Page,
            filter.PageSize,
            filter.SortBy,
            filter.IsAscending,
            filter.Filter,
            PersonMapper.ToArrayResponse,
            cancellationToken);

        return HandleResult(paginatedPeople);
    }

    [HttpGet("{personId}")]
    [ProducesResponseType<PaginatedResult<PersonResponse[]>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status403Forbidden)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetPersonAsync(
        Guid tenantId,
        Guid personId,
        CancellationToken cancellationToken)
    {
        Result<PersonResponse> person = await _personUseCase.GetByIdAsync(
            tenantId,
            personId,
            PersonMapper.ToResponse,
            cancellationToken);

        return HandleResult(person);
    }

    [HttpPost]
    [ProducesResponseType<Result<PersonResponse>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status403Forbidden)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreatePersonAsync(
        Guid tenantId,
        CreatePersonRequest request,
        CancellationToken cancellationToken = default)
    {
        PersonCreateModel personCreateModel = PersonMapper.ToCreateModel(request, tenantId);
        AuditStampCreateModel auditLogCreateModel = AuditLogMapper.ToCreateModel(
            Guid.NewGuid(),
            "asd@asd.dasd",
            tenantId);

        Result<Person> createdPerson = await _personUseCase.CreateAsync(
            personCreateModel,
            auditLogCreateModel,
            cancellationToken);

        return HandleResult(
            result: createdPerson,
            mapper: PersonMapper.ToResponse);
    }

    [HttpPatch("{personId}")]
    [ProducesResponseType<Result<PersonResponse>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status403Forbidden)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdatePersonAsync(
        Guid tenantId,
        Guid personId,
        UpdatePersonRequest request,
        CancellationToken cancellationToken = default)
    {
        PersonUpdateModel personUpdateModel = PersonMapper.ToUpdateModel(request, personId);
        AuditStampCreateModel auditStampCreateModel = AuditLogMapper.ToCreateModel(
            Guid.NewGuid(),
            "asd@asd.dasd",
            tenantId);

        Result updatedPerson = await _personUseCase.UpdateAsync(
            tenantId,
            personUpdateModel,
            auditStampCreateModel,
            cancellationToken);

        return HandleResult(result: updatedPerson);
    }

    [HttpDelete("{personId}")]
    [ProducesResponseType<Result<PersonResponse>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status403Forbidden)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeletePersonAsync(
        Guid tenantId,
        Guid personId,
        CancellationToken cancellationToken = default)
    {
        AuditStampCreateModel auditStampCreateModel = AuditLogMapper.ToCreateModel(
            Guid.NewGuid(),
            "asd@asd.dasd",
            tenantId);

        Result deletedPerson = await _personUseCase.DeleteAsync(
            tenantId,
            personId,
            auditStampCreateModel,
            cancellationToken);

        return HandleResult(result: deletedPerson);
    }
}

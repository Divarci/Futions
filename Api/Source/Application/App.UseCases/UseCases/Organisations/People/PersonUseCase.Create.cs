using App.UseCases.Helpers;
using Core.Domain.Entities.System.AuditLogs;
using Core.Domain.Entities.Organisations.People;
using Core.Domain.Entities.Organisations.People.Models;
using Core.Domain.ValueObjects.AuditStampValueObject;
using Core.Library.ResultPattern;
using Microsoft.Extensions.Logging;

namespace App.UseCases.UseCases.Organisations.People;

internal sealed partial class PersonUseCase
{
    public async Task<Result<Person>> CreatePersonAsync(
        PersonCreateModel createModel,
        AuditStampCreateModel auditStampCreateModel,
        CancellationToken cancellationToken = default)
    {
        return await _unitOfWork.ExecuteTransactionAsync(async () =>
        {
            Result<Person> personCreateResult = await _personService
                .CreatePersonAsync(createModel, CacheKeyHelper.Single, cancellationToken);

            if (personCreateResult.IsFailureAndNoData)
                return personCreateResult;

            Result<AuditLog> auditLogCreateResult = await _auditLogService
                .CreateAsync(
                    personCreateResult.Data.Id,
                    $"Person with ID {personCreateResult.Data.Id} has been created by {auditStampCreateModel.Username}.",
                    auditStampCreateModel,
                    CacheKeyHelper.Single,
                    cancellationToken);

            if (auditLogCreateResult.IsFailureAndNoData)
            {
                string traceId = Guid.NewGuid().ToString();
                _logger.LogWarning(
                    "Audit log creation failed for person {PersonId}. {Message} | TraceId: {TraceId}",
                    personCreateResult.Data.Id,
                    auditLogCreateResult.Message,
                    traceId);
            }

            return personCreateResult;
        }, cancellationToken);
    }
}

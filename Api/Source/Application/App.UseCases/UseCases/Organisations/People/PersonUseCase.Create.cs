using Core.Domain.Entities.Auditing.AuditLogs;
using Core.Domain.Entities.Organisations.People;
using Core.Domain.Entities.Organisations.People.Models;
using Core.Domain.ValueObjects.AuditStampValueObject;
using Core.Library.ResultPattern;

namespace App.UseCases.UseCases.Organisations.People;

internal sealed partial class PersonUseCase
{
    public async Task<Result<Person>> CreateAsync(
        PersonCreateModel createModel,
        AuditStampCreateModel auditStampCreateModel,
        CancellationToken cancellationToken = default)
    {
        return await _unitOfWork.ExecuteTransactionAsync(async () =>
        {
            Result<Person> personCreateResult = await _personService
                .CreateAsync(createModel, cancellationToken);

            if (personCreateResult.IsFailureAndNoData)
                return personCreateResult;

            Result<AuditLog> auditLogCreateResult = await _auditLogService
                .CreateAsync(
                    personCreateResult.Data.Id,
                    $"Person with ID {personCreateResult.Data.Id} has been created by {auditStampCreateModel.Username}.",
                    auditStampCreateModel,
                    cancellationToken);

            if (auditLogCreateResult.IsFailureAndNoData)
            {
                // Just we need logging here
            }

            return personCreateResult;
        }, cancellationToken);
    }
}

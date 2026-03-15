using Core.Domain.Entities.Auditing.AuditLogs;
using Core.Domain.Entities.Organisations.People.Models;
using Core.Domain.ValueObjects.AuditStampValueObject;
using Core.Library.ResultPattern;

namespace App.UseCases.UseCases.Organisations.People;

internal sealed partial class PersonUseCase
{
    public async Task<Result> UpdateAsync(
        Guid tenantId,
        PersonUpdateModel updateModel,
        AuditStampCreateModel auditStampCreateModel,
        CancellationToken cancellationToken = default)
    {
        return await _unitOfWork.ExecuteTransactionAsync(async () =>
        {
            Result personUpdateResult = await _personService
                .UpdateAsync(tenantId, updateModel, cancellationToken);

            if (personUpdateResult.IsFailure)
                return personUpdateResult;

            Result<AuditLog> auditLogCreateResult = await _auditLogService
                .CreateAsync(
                    updateModel.PersonId,
                    $"Person with ID {updateModel.PersonId} has been updated by {auditStampCreateModel.Username}.",
                    auditStampCreateModel,
                    cancellationToken);

            if (auditLogCreateResult.IsFailureAndNoData)
            {
                // Just we need logging here
            }

            return personUpdateResult;
        }, cancellationToken);
    }
}

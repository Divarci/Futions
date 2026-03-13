using Core.Domain.Entities.Auditing.AuditLogs;
using Core.Domain.Entities.Organisations.People.Models;
using Core.Domain.Entities.System.AuditLogs.Models;
using Core.Library.ResultPattern;

namespace App.UseCases.UseCases.Organisations.People;

internal sealed partial class PersonUseCase
{
    public async Task<Result> UpdateAsync(
        Guid tenantId,
        PersonUpdateModel updateModel,
        AuditLogCreateModel auditLogCreateModel,
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
                    tenantId,
                    updateModel.PersonId,
                    $"Person with ID {updateModel.PersonId} has been updated by {auditLogCreateModel.CreatedStampModel.Username}.",
                    auditLogCreateModel,
                    cancellationToken);

            if (auditLogCreateResult.IsFailureAndNoData)
            {
                // Just we need logging here
            }

            return personUpdateResult;
        }, cancellationToken);
    }
}

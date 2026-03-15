using Core.Domain.Entities.Auditing.AuditLogs;
using Core.Domain.ValueObjects.AuditStampValueObject;
using Core.Library.ResultPattern;

namespace App.UseCases.UseCases.Organisations.People;

internal sealed partial class PersonUseCase
{
    public async Task<Result> DeleteAsync(
        Guid tenantId,
        Guid id,
        AuditStampCreateModel auditStampCreateModel,
        CancellationToken cancellationToken = default)
    {
        return await _unitOfWork.ExecuteTransactionAsync(async () =>
        {
            Result personDeleteResult = await _personService
                .DeleteAsync(tenantId, id, cancellationToken);

            if (personDeleteResult.IsFailure)
                return personDeleteResult;

            Result<AuditLog> auditLogCreateResult = await _auditLogService
                .CreateAsync(
                    id,
                    $"Person with ID {id} has been deleted by {auditStampCreateModel.Username}.",
                    auditStampCreateModel,
                    cancellationToken);

            if (auditLogCreateResult.IsFailureAndNoData)
            {
                // Just we need logging here
            }

            return personDeleteResult;
        }, cancellationToken);
    }
}

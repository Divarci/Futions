using App.UseCases.Helpers;
using Core.Domain.Entities.Auditing.AuditLogs;
using Core.Domain.ValueObjects.AuditStampValueObject;
using Core.Library.ResultPattern;
using Microsoft.Extensions.Logging;

namespace App.UseCases.UseCases.Organisations.People;

internal sealed partial class PersonUseCase
{
    public async Task<Result> DeletePersonAsync(
        Guid tenantId,
        Guid personId,
        AuditStampCreateModel auditStampCreateModel,
        CancellationToken cancellationToken = default)
    {
        return await _unitOfWork.ExecuteTransactionAsync(async () =>
        {
            // Delete person
            Result personDeleteResult = await _personService
                .DeletePersonAsync(tenantId, personId, CacheKeyHelper.Single, cancellationToken);

            if (personDeleteResult.IsFailure)
                return personDeleteResult;

            // Create audit log
            Result<AuditLog> auditLogCreateResult = await _auditLogService
                .CreateAsync(
                    personId,
                    $"Person with ID {personId} has been deleted by {auditStampCreateModel.Username}.",
                    auditStampCreateModel,
                    cancellationToken);

            if (auditLogCreateResult.IsFailureAndNoData)
            {
                string traceId = Guid.NewGuid().ToString();
                _logger.LogWarning(
                    "Audit log creation failed for person {PersonId}. {Message} | TraceId: {TraceId}",
                    personId,
                    auditLogCreateResult.Message,
                    traceId);
            }

            return personDeleteResult;
        }, cancellationToken);
    }
}

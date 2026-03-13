using Core.Domain.Entities.Auditing.AuditLogs;
using Core.Domain.Entities.Organisations.People;
using Core.Domain.Entities.Organisations.People.Models;
using Core.Domain.Entities.System.AuditLogs.Models;
using Core.Library.ResultPattern;

namespace App.UseCases.UseCases.Organisations.People;

internal sealed partial class PersonUseCase
{
    public async Task<Result<Person>> CreateAsync(
        Guid tenantId,
        PersonCreateModel createModel,
        AuditLogCreateModel auditLogCreateModel,
        CancellationToken cancellationToken = default)
    {
        return await _unitOfWork.ExecuteTransactionAsync(async () =>
        {
            Result<Person> personCreateResult = await _personService
                .CreateAsync(tenantId, createModel, cancellationToken);

            if (personCreateResult.IsFailureAndNoData)
                return personCreateResult;

            Result<AuditLog> auditLogCreateResult = await _auditLogService
                .CreateAsync(tenantId, auditLogCreateModel, cancellationToken);

            if (auditLogCreateResult.IsFailureAndNoData)
            {
                // Just we need logging here
            }

            return personCreateResult;
        }, cancellationToken);
    }
}

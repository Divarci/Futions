using Core.Domain.Entities.Auditing.AuditLogs;
using Core.Domain.ValueObjects.AuditStampValueObject;
using Core.Library.ResultPattern;

namespace App.UseCases.UseCases.Organisations.Companies;

internal sealed partial class CompanyUseCase
{
    public async Task<Result> DeleteAsync(
        Guid tenantId,
        Guid id,
        AuditStampCreateModel auditStampCreateModel,
        CancellationToken cancellationToken = default)
    {
        return await _unitOfWork.ExecuteTransactionAsync(async () =>
        {
            // Delete company
            Result companyDeleteResult = await _companyService
                .DeleteAsync(tenantId, id, cancellationToken);

            if (companyDeleteResult.IsFailure)
                return companyDeleteResult;

            // Create audit log
            Result<AuditLog> auditLogCreateResult = await _auditLogService
                .CreateAsync(
                    id,
                    $"Company with ID {id} has been deleted by {auditStampCreateModel.Username}.",
                    auditStampCreateModel, 
                    cancellationToken);

            if (auditLogCreateResult.IsFailureAndNoData)
            {
                // Just we need logging here
            }

            return companyDeleteResult;

        }, cancellationToken);
    }
}

using Core.Domain.Entities.Auditing.AuditLogs;
using Core.Domain.Entities.System.AuditLogs.Models;
using Core.Library.ResultPattern;

namespace App.UseCases.UseCases.Organisations.Companies;

internal sealed partial class CompanyUseCase
{
    public async Task<Result> DeleteAsync(
        Guid tenantId,
        Guid id,
        AuditLogCreateModel auditLogCreateModel,
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
                    tenantId,
                    id,
                    $"Company with ID {id} has been deleted by {auditLogCreateModel.CreatedStampModel.Username}.",
                    auditLogCreateModel, 
                    cancellationToken);

            if (auditLogCreateResult.IsFailureAndNoData)
            {
                // Just we need logging here
            }

            return companyDeleteResult;

        }, cancellationToken);
    }
}

using Core.Domain.Entities.Auditing.AuditLogs;
using Core.Domain.Entities.System.AuditLogs.Models;
using Core.Library.ResultPattern;

namespace App.UseCases.UseCases.Organisations.CompanyPeople;

internal sealed partial class CompanyPersonUseCase
{
    public async Task<Result> DeleteAsync(
        Guid tenantId,
        Guid id,
        AuditLogCreateModel auditLogCreateModel,
        CancellationToken cancellationToken = default)
    {
        return await _unitOfWork.ExecuteTransactionAsync(async () =>
        {
            // Delete company person.
            Result companyPersonDeleteResult = await _companyPersonService
                .DeleteAsync(tenantId, id, cancellationToken);

            if (companyPersonDeleteResult.IsFailure)
                return companyPersonDeleteResult;

            // Create audit log.
            Result<AuditLog> auditLogCreateResult = await _auditLogService
                .CreateAsync(
                    tenantId,
                    id,
                    $"Company person with ID {id} has been deleted by {auditLogCreateModel.CreatedStampModel.Username}.",
                    auditLogCreateModel,
                    cancellationToken);

            if (auditLogCreateResult.IsFailureAndNoData)
            {
                // Just we need logging here.
            }

            return companyPersonDeleteResult;
        }, cancellationToken);
    }
}

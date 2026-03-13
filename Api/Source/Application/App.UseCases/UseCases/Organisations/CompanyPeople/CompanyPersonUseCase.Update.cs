using Core.Domain.Entities.Auditing.AuditLogs;
using Core.Domain.Entities.Organisations.CompanyPeople.Models;
using Core.Domain.Entities.System.AuditLogs.Models;
using Core.Library.ResultPattern;

namespace App.UseCases.UseCases.Organisations.CompanyPeople;

internal sealed partial class CompanyPersonUseCase
{
    public async Task<Result> UpdateAsync(
        Guid tenantId,
        CompanyPersonUpdateModel updateModel,
        AuditLogCreateModel auditLogCreateModel,
        CancellationToken cancellationToken = default)
    {
        return await _unitOfWork.ExecuteTransactionAsync(async () =>
        {
            // Update company person.
            Result companyPersonUpdateResult = await _companyPersonService
                .UpdateAsync(tenantId, updateModel, cancellationToken);

            if (companyPersonUpdateResult.IsFailure)
                return companyPersonUpdateResult;

            // Create audit log.
            Result<AuditLog> auditLogCreateResult = await _auditLogService
                .CreateAsync(
                    tenantId,
                    updateModel.CompanyPersonId,
                    $"Company person with ID {updateModel.CompanyPersonId} has been updated by {auditLogCreateModel.CreatedStampModel.Username}.",
                    auditLogCreateModel,
                    cancellationToken);

            if (auditLogCreateResult.IsFailureAndNoData)
            {
                // Just we need logging here.
            }

            return companyPersonUpdateResult;
        }, cancellationToken);
    }
}

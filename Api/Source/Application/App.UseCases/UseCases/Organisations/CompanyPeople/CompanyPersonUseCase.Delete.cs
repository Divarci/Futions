using Core.Domain.Entities.Auditing.AuditLogs;
using Core.Domain.ValueObjects.AuditStampValueObject;
using Core.Library.ResultPattern;

namespace App.UseCases.UseCases.Organisations.CompanyPeople;

internal sealed partial class CompanyPersonUseCase
{
    public async Task<Result> DeleteAsync(
        Guid tenantId,
        Guid id,
        AuditStampCreateModel auditStampCreateModel,
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
                    id,
                    $"Company person with ID {id} has been deleted by {auditStampCreateModel.Username}.",
                    auditStampCreateModel,
                    cancellationToken);

            if (auditLogCreateResult.IsFailureAndNoData)
            {
                // Just we need logging here.
            }

            return companyPersonDeleteResult;
        }, cancellationToken);
    }
}

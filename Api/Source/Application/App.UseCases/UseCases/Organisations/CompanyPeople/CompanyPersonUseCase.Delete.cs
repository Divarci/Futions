using Core.Domain.Entities.Auditing.AuditLogs;
using Core.Domain.ValueObjects.AuditStampValueObject;
using Core.Library.ResultPattern;

namespace App.UseCases.UseCases.Organisations.CompanyPeople;

internal sealed partial class CompanyPersonUseCase
{
    public async Task<Result> DeleteCompanyPersonAsync(
        Guid tenantId,
        Guid companyId,
        Guid companyPersonId,
        AuditStampCreateModel auditStampCreateModel,
        CancellationToken cancellationToken = default)
    {
        return await _unitOfWork.ExecuteTransactionAsync(async () =>
        {
            // Delete company person.
            Result companyPersonDeleteResult = await _companyPersonService
                .DeleteCompanyPersonAsync(tenantId, companyId, companyPersonId, cancellationToken);

            if (companyPersonDeleteResult.IsFailure)
                return companyPersonDeleteResult;

            // Create audit log.
            Result<AuditLog> auditLogCreateResult = await _auditLogService
                .CreateAsync(
                    companyPersonId,
                    $"Company person with ID {companyPersonId} has been deleted by {auditStampCreateModel.Username}.",
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

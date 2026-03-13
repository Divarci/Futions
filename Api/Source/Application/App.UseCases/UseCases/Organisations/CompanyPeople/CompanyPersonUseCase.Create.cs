using Core.Domain.Entities.Auditing.AuditLogs;
using Core.Domain.Entities.Organisations.CompanyPeople;
using Core.Domain.Entities.Organisations.CompanyPeople.Models;
using Core.Domain.Entities.System.AuditLogs.Models;
using Core.Library.ResultPattern;

namespace App.UseCases.UseCases.Organisations.CompanyPeople;

internal sealed partial class CompanyPersonUseCase
{
    public async Task<Result<CompanyPerson>> CreateAsync(
        Guid tenantId,
        CompanyPersonCreateModel createModel,
        AuditLogCreateModel auditLogCreateModel,
        CancellationToken cancellationToken = default)
    {
        return await _unitOfWork.ExecuteTransactionAsync(async () =>
        {
            // Create company person.
            Result<CompanyPerson> companyPersonCreateResult = await _companyPersonService
                .CreateAsync(tenantId, createModel, cancellationToken);

            if (companyPersonCreateResult.IsFailureAndNoData)
                return companyPersonCreateResult;

            // Create audit log.
            Result<AuditLog> auditLogCreateResult = await _auditLogService
                .CreateAsync(
                    tenantId,
                    companyPersonCreateResult.Data.Id,
                    $"Company person with ID {companyPersonCreateResult.Data.Id} has been created by {auditLogCreateModel.CreatedStampModel.Username}.",
                    auditLogCreateModel,
                    cancellationToken);

            if (auditLogCreateResult.IsFailureAndNoData)
            {
                // Just we need logging here.
            }

            return companyPersonCreateResult;
        }, cancellationToken);
    }
}

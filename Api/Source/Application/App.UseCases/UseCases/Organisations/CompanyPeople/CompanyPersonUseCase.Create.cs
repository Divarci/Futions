using Core.Domain.Entities.Auditing.AuditLogs;
using Core.Domain.Entities.Organisations.CompanyPeople;
using Core.Domain.Entities.Organisations.CompanyPeople.Models;
using Core.Domain.ValueObjects.AuditStampValueObject;
using Core.Library.ResultPattern;

namespace App.UseCases.UseCases.Organisations.CompanyPeople;

internal sealed partial class CompanyPersonUseCase
{
    public async Task<Result<CompanyPerson>> CreateAsync(
        CompanyPersonCreateModel createModel,
        AuditStampCreateModel auditStampCreateModel,
        CancellationToken cancellationToken = default)
    {
        return await _unitOfWork.ExecuteTransactionAsync(async () =>
        {
            // Create company person.
            Result<CompanyPerson> companyPersonCreateResult = await _companyPersonService
                .CreateAsync(auditStampCreateModel.TenantId, createModel, cancellationToken);

            if (companyPersonCreateResult.IsFailureAndNoData)
                return companyPersonCreateResult;

            // Create audit log.
            Result<AuditLog> auditLogCreateResult = await _auditLogService
                .CreateAsync(
                    companyPersonCreateResult.Data.Id,
                    $"Company person with ID {companyPersonCreateResult.Data.Id} has been created by {auditStampCreateModel.Username}.",
                    auditStampCreateModel,
                    cancellationToken);

            if (auditLogCreateResult.IsFailureAndNoData)
            {
                // Just we need logging here.
            }

            return companyPersonCreateResult;
        }, cancellationToken);
    }
}

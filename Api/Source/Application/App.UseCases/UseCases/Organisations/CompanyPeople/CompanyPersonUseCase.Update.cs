using Core.Domain.Entities.Auditing.AuditLogs;
using Core.Domain.Entities.Organisations.CompanyPeople.Models;
using Core.Domain.ValueObjects.AuditStampValueObject;
using Core.Library.ResultPattern;
using Microsoft.Extensions.Logging;

namespace App.UseCases.UseCases.Organisations.CompanyPeople;

internal sealed partial class CompanyPersonUseCase
{
    public async Task<Result> UpdateCompanyPersonAsync(
        CompanyPersonUpdateModel updateModel,
        AuditStampCreateModel auditStampCreateModel,
        CancellationToken cancellationToken = default)
    {
        return await _unitOfWork.ExecuteTransactionAsync(async () =>
        {
            // Update company person.
            Result companyPersonUpdateResult = await _companyPersonService
                .UpdateCompanyPersonAsync(updateModel, cancellationToken);

            if (companyPersonUpdateResult.IsFailure)
                return companyPersonUpdateResult;

            // Create audit log.
            Result<AuditLog> auditLogCreateResult = await _auditLogService
                .CreateAsync(
                    updateModel.CompanyPersonId,
                    $"Company person with ID {updateModel.CompanyPersonId} has been updated by {auditStampCreateModel.Username}.",
                    auditStampCreateModel,
                    cancellationToken);

            if (auditLogCreateResult.IsFailureAndNoData)            
                _logger.LogWarning(
                    "Audit log creation failed for company person {CompanyPersonId}. {Message}",
                    updateModel.CompanyPersonId,
                    auditLogCreateResult.Message);            

            return companyPersonUpdateResult;
        }, cancellationToken);
    }
}

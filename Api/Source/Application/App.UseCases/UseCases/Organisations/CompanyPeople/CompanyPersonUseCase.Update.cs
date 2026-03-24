using App.UseCases.Helpers;
using Core.Domain.Entities.System.AuditLogs;
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
                    CacheKeyHelper.Single,
                    cancellationToken);

            if (auditLogCreateResult.IsFailureAndNoData)
            {
                string traceId = Guid.NewGuid().ToString();
                _logger.LogWarning(
                    "Audit log creation failed for company person {CompanyPersonId}. {Message} | TraceId: {TraceId}",
                    updateModel.CompanyPersonId,
                    auditLogCreateResult.Message,
                    traceId);
            }

            return companyPersonUpdateResult;
        }, cancellationToken);
    }
}

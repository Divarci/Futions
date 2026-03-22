using App.UseCases.Helpers;
using Core.Domain.Entities.Auditing.AuditLogs;
using Core.Domain.ValueObjects.AuditStampValueObject;
using Core.Library.ResultPattern;
using Microsoft.Extensions.Logging;

namespace App.UseCases.UseCases.Organisations.Companies;

internal sealed partial class CompanyUseCase
{
    public async Task<Result> DeleteCompanyAsync(
        Guid tenantId,
        Guid companyId,
        AuditStampCreateModel auditStampCreateModel,
        CancellationToken cancellationToken = default)
    {
        return await _unitOfWork.ExecuteTransactionAsync(async () =>
        {
            // Delete company
            Result companyDeleteResult = await _companyService
                .DeleteCompanyAsync(tenantId, companyId, CacheKeyHelper.Single, cancellationToken);

            if (companyDeleteResult.IsFailure)
                return companyDeleteResult;

            // Create audit log
            Result<AuditLog> auditLogCreateResult = await _auditLogService
                .CreateAsync(
                    companyId,
                    $"Company with ID {companyId} has been deleted by {auditStampCreateModel.Username}.",
                    auditStampCreateModel, 
                    cancellationToken);

            if (auditLogCreateResult.IsFailureAndNoData)
            {
                string traceId = Guid.NewGuid().ToString();
                _logger.LogWarning(
                    "Audit log creation failed for company {CompanyId}. {Message} | TraceId: {TraceId}",
                    companyId,
                    auditLogCreateResult.Message,
                    traceId);
            }

            return companyDeleteResult;

        }, cancellationToken);
    }
}

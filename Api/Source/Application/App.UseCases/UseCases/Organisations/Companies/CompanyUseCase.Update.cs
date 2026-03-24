using App.UseCases.Helpers;
using Core.Domain.Entities.System.AuditLogs;
using Core.Domain.Entities.Organisations.Companies.Models;
using Core.Domain.ValueObjects.AuditStampValueObject;
using Core.Library.ResultPattern;
using Microsoft.Extensions.Logging;

namespace App.UseCases.UseCases.Organisations.Companies;

internal sealed partial class CompanyUseCase
{
        public async Task<Result> UpdateCompanyAsync(
            CompanyUpdateModel updateModel,
            AuditStampCreateModel auditStampCreateModel,
            CancellationToken cancellationToken = default)
        {
            return await _unitOfWork.ExecuteTransactionAsync(async () =>
            {
                // Update company
                Result companyUpdateResult = await _companyService
                    .UpdateCompanyAsync(updateModel, CacheKeyHelper.Single, cancellationToken);
    
                if (companyUpdateResult.IsFailure)
                    return companyUpdateResult;
    
                // Create audit log
                Result<AuditLog> auditLogCreateResult = await _auditLogService
                    .CreateAsync(
                        updateModel.CompanyId,
                        $"Company with ID {updateModel.CompanyId} has been updated by {auditStampCreateModel.Username}.",
                        auditStampCreateModel,
                        CacheKeyHelper.Single,
                        cancellationToken);
    
                if (auditLogCreateResult.IsFailureAndNoData)
                {
                    string traceId = Guid.NewGuid().ToString();
                    _logger.LogWarning(
                        "Audit log creation failed for company {CompanyId}. {Message} | TraceId: {TraceId}",
                        updateModel.CompanyId,
                        auditLogCreateResult.Message,
                        traceId);
                }
    
                return companyUpdateResult;
            }, cancellationToken);
    }
}

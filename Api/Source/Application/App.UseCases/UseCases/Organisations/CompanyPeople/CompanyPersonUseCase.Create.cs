using App.UseCases.Helpers;
using Core.Domain.Entities.Auditing.AuditLogs;
using Core.Domain.Entities.Organisations.CompanyPeople;
using Core.Domain.Entities.Organisations.CompanyPeople.Models;
using Core.Domain.ValueObjects.AuditStampValueObject;
using Core.Library.ResultPattern;
using Microsoft.Extensions.Logging;

namespace App.UseCases.UseCases.Organisations.CompanyPeople;

internal sealed partial class CompanyPersonUseCase
{
    public async Task<Result<CompanyPerson>> CreateCompanyPersonAsync(
        CompanyPersonCreateModel createModel,
        AuditStampCreateModel auditStampCreateModel,
        CancellationToken cancellationToken = default)
    {
        return await _unitOfWork.ExecuteTransactionAsync(async () =>
        {
            // Create company person.
            Result<CompanyPerson> companyPersonCreateResult = await _companyPersonService
                .CreateCompanyPersonAsync(auditStampCreateModel.TenantId, createModel, CacheKeyHelper.Single, cancellationToken);

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
                string traceId = Guid.NewGuid().ToString();
                _logger.LogWarning(
                    "Audit log creation failed for company person {CompanyPersonId}. {Message} | TraceId: {TraceId}",
                    companyPersonCreateResult.Data.Id,
                    auditLogCreateResult.Message,
                    traceId);
            }

            return companyPersonCreateResult;
        }, cancellationToken);
    }
}

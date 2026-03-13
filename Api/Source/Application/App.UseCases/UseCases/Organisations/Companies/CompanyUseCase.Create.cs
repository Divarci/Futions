using Core.Domain.Entities.Auditing.AuditLogs;
using Core.Domain.Entities.Organisations.Companies;
using Core.Domain.Entities.Organisations.Companies.Models;
using Core.Domain.Entities.System.AuditLogs.Models;
using Core.Library.ResultPattern;

namespace App.UseCases.UseCases.Organisations.Companies;

internal sealed partial class CompanyUseCase
{
    public async Task<Result<Company>> CreateAsync(
        Guid tenantId,
        CompanyCreateModel createModel,
        AuditLogCreateModel auditLogCreateModel,
        CancellationToken cancellationToken = default)
    {
        return await _unitOfWork.ExecuteTransactionAsync(async () =>
        {
            // Create company
            Result<Company> companyCreateResult = await _companyService
                .CreateAsync(tenantId, createModel, cancellationToken);

            if (companyCreateResult.IsFailureAndNoData)
                return companyCreateResult;

            // Create audit log
            Result<AuditLog> auditLogCreateResult = await _auditLogService
                .CreateAsync(
                    tenantId,
                    companyCreateResult.Data.Id,
                    $"Company created with name: {companyCreateResult.Data.Name} by {auditLogCreateModel.CreatedStampModel.Username}",
                    auditLogCreateModel, 
                    cancellationToken);

            if (auditLogCreateResult.IsFailureAndNoData)
            {
                // Just we need logging here
            }

            return companyCreateResult;
        }, cancellationToken);
    }
}


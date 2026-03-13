using Core.Domain.Entities.Auditing.AuditLogs;
using Core.Domain.Entities.Organisations.Companies;
using Core.Domain.Entities.Organisations.Companies.Models;
using Core.Domain.Entities.System.AuditLogs.Models;
using Core.Library.ResultPattern;

namespace App.UseCases.UseCases.Organisations.Companies;

internal sealed partial class CompanyUseCase
{
        public async Task<Result> UpdateAsync(
            Guid tenantId,
            CompanyUpdateModel updateModel,
            AuditLogCreateModel auditLogCreateModel,
            CancellationToken cancellationToken = default)
        {
            return await _unitOfWork.ExecuteTransactionAsync(async () =>
            {
                // Update company
                Result companyUpdateResult = await _companyService
                    .UpdateAsync(tenantId, updateModel, cancellationToken);
    
                if (companyUpdateResult.IsFailure)
                    return companyUpdateResult;
    
                // Create audit log
                Result<AuditLog> auditLogCreateResult = await _auditLogService
                    .CreateAsync(tenantId, auditLogCreateModel, cancellationToken);
    
                if (auditLogCreateResult.IsFailureAndNoData)
                {
                    // Just we need logging here
                }
    
                return companyUpdateResult;
            }, cancellationToken);
    }
}

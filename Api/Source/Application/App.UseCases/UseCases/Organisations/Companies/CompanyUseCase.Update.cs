using Core.Domain.Entities.Auditing.AuditLogs;
using Core.Domain.Entities.Organisations.Companies.Models;
using Core.Domain.ValueObjects.AuditStampValueObject;
using Core.Library.ResultPattern;

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
                    .UpdateCompanyAsync(updateModel, cancellationToken);
    
                if (companyUpdateResult.IsFailure)
                    return companyUpdateResult;
    
                // Create audit log
                Result<AuditLog> auditLogCreateResult = await _auditLogService
                    .CreateAsync(
                        updateModel.CompanyId,
                        $"Company with ID {updateModel.CompanyId} has been updated by {auditStampCreateModel.Username}.",
                        auditStampCreateModel, 
                        cancellationToken);
    
                if (auditLogCreateResult.IsFailureAndNoData)
                {
                    // Just we need logging here
                }
    
                return companyUpdateResult;
            }, cancellationToken);
    }
}

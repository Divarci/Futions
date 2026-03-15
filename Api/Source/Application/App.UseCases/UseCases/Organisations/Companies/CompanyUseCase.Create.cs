using Core.Domain.Entities.Auditing.AuditLogs;
using Core.Domain.Entities.Organisations.Companies;
using Core.Domain.Entities.Organisations.Companies.Models;
using Core.Domain.ValueObjects.AuditStampValueObject;
using Core.Library.ResultPattern;

namespace App.UseCases.UseCases.Organisations.Companies;

internal sealed partial class CompanyUseCase
{
    public async Task<Result<Company>> CreateAsync(
        CompanyCreateModel createModel,
        AuditStampCreateModel auditStampCreateModel,
        CancellationToken cancellationToken = default)
    {
        return await _unitOfWork.ExecuteTransactionAsync(async () =>
        {
            // Create company
            Result<Company> companyCreateResult = await _companyService
                .CreateAsync(createModel, cancellationToken);

            if (companyCreateResult.IsFailureAndNoData)
                return companyCreateResult;

            // Create audit log
            Result<AuditLog> auditLogCreateResult = await _auditLogService
                .CreateAsync(
                    companyCreateResult.Data.Id,
                    $"Company created with name: {companyCreateResult.Data.Name} by {auditStampCreateModel.Username}",
                    auditStampCreateModel, 
                    cancellationToken);

            if (auditLogCreateResult.IsFailureAndNoData)
            {
                // Just we need logging here
            }

            return companyCreateResult;
        }, cancellationToken);
    }
}


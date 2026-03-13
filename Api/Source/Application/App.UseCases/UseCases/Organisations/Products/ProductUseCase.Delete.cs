using Core.Domain.Entities.Auditing.AuditLogs;
using Core.Domain.Entities.System.AuditLogs.Models;
using Core.Library.ResultPattern;

namespace App.UseCases.UseCases.Organisations.Products;

internal sealed partial class ProductUseCase
{
    public async Task<Result> DeleteAsync(
        Guid tenantId,
        Guid id,
        AuditLogCreateModel auditLogCreateModel,
        CancellationToken cancellationToken = default)
    {
        return await _unitOfWork.ExecuteTransactionAsync(async () =>
        {
            // Delete product.
            Result productDeleteResult = await _productService
                .DeleteAsync(tenantId, id, cancellationToken);

            if (productDeleteResult.IsFailure)
                return productDeleteResult;

            // Create audit log.
            Result<AuditLog> auditLogCreateResult = await _auditLogService
                .CreateAsync(
                    tenantId,
                    id,
                    $"Product with ID {id} has been deleted by {auditLogCreateModel.CreatedStampModel.Username}.",
                    auditLogCreateModel,
                    cancellationToken);

            if (auditLogCreateResult.IsFailureAndNoData)
            {
                // Just we need logging here.
            }

            return productDeleteResult;
        }, cancellationToken);
    }
}

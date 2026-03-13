using Core.Domain.Entities.Auditing.AuditLogs;
using Core.Domain.Entities.Organisations.Products;
using Core.Domain.Entities.Organisations.Products.Models;
using Core.Domain.Entities.System.AuditLogs.Models;
using Core.Library.ResultPattern;

namespace App.UseCases.UseCases.Organisations.Products;

internal sealed partial class ProductUseCase
{
    public async Task<Result<Product>> UpdateAsync(
        Guid tenantId,
        ProductUpdateModel updateModel,
        AuditLogCreateModel auditLogCreateModel,
        CancellationToken cancellationToken = default)
    {
        return await _unitOfWork.ExecuteTransactionAsync(async () =>
        {
            // Update product.
            Result<Product> productUpdateResult = await _productService
                .UpdateAsync(tenantId, updateModel, cancellationToken);

            if (productUpdateResult.IsFailureAndNoData)
                return productUpdateResult;

            // Create audit log.
            Result<AuditLog> auditLogCreateResult = await _auditLogService
                .CreateAsync(
                    tenantId,
                    updateModel.ProductId,
                    $"Product with ID {updateModel.ProductId} has been updated by {auditLogCreateModel.CreatedStampModel.Username}.",
                    auditLogCreateModel,
                    cancellationToken);

            if (auditLogCreateResult.IsFailureAndNoData)
            {
                // Just we need logging here.
            }

            return productUpdateResult;
        }, cancellationToken);
    }
}

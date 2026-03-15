using Core.Domain.Entities.Auditing.AuditLogs;
using Core.Domain.Entities.Organisations.Products;
using Core.Domain.Entities.Organisations.Products.Models;
using Core.Domain.ValueObjects.AuditStampValueObject;
using Core.Library.ResultPattern;

namespace App.UseCases.UseCases.Organisations.Products;

internal sealed partial class ProductUseCase
{
    public async Task<Result<Product>> UpdateAsync(
        Guid tenantId,
        ProductUpdateModel updateModel,
        AuditStampCreateModel auditStampCreateModel,
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
                    updateModel.ProductId,
                    $"Product with ID {updateModel.ProductId} has been updated by {auditStampCreateModel.Username}.",
                    auditStampCreateModel,
                    cancellationToken);

            if (auditLogCreateResult.IsFailureAndNoData)
            {
                // Just we need logging here.
            }

            return productUpdateResult;
        }, cancellationToken);
    }
}

using Core.Domain.Entities.Auditing.AuditLogs;
using Core.Domain.Entities.Organisations.Products;
using Core.Domain.Entities.Organisations.Products.Models;
using Core.Domain.Entities.System.AuditLogs.Models;
using Core.Library.ResultPattern;

namespace App.UseCases.UseCases.Organisations.Products;

internal sealed partial class ProductUseCase
{
    public async Task<Result<Product>> CreateAsync(
        Guid tenantId,
        ProductCreateModel createModel,
        AuditLogCreateModel auditLogCreateModel,
        CancellationToken cancellationToken = default)
    {
        return await _unitOfWork.ExecuteTransactionAsync(async () =>
        {
            // Create product.
            Result<Product> productCreateResult = await _productService
                .CreateAsync(tenantId, createModel, cancellationToken);

            if (productCreateResult.IsFailureAndNoData)
                return productCreateResult;

            // Create audit log.
            Result<AuditLog> auditLogCreateResult = await _auditLogService
                .CreateAsync(
                    tenantId,
                    productCreateResult.Data.Id,
                    $"Product with ID {productCreateResult.Data.Id} has been created by {auditLogCreateModel.CreatedStampModel.Username}.",
                    auditLogCreateModel,
                    cancellationToken);

            if (auditLogCreateResult.IsFailureAndNoData)
            {
                // Just we need logging here.
            }

            return productCreateResult;
        }, cancellationToken);
    }
}

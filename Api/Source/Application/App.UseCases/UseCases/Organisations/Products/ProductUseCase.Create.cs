using Core.Domain.Entities.Auditing.AuditLogs;
using Core.Domain.Entities.Organisations.Products;
using Core.Domain.Entities.Organisations.Products.Models;
using Core.Domain.ValueObjects.AuditStampValueObject;
using Core.Library.ResultPattern;

namespace App.UseCases.UseCases.Organisations.Products;

internal sealed partial class ProductUseCase
{
    public async Task<Result<Product>> CreateAsync(
        ProductCreateModel createModel,
        AuditStampCreateModel auditStampCreateModel,
        CancellationToken cancellationToken = default)
    {
        return await _unitOfWork.ExecuteTransactionAsync(async () =>
        {
            // Create product.
            Result<Product> productCreateResult = await _productService
                .CreateAsync(createModel, cancellationToken);

            if (productCreateResult.IsFailureAndNoData)
                return productCreateResult;

            // Create audit log.
            Result<AuditLog> auditLogCreateResult = await _auditLogService
                .CreateAsync(
                    productCreateResult.Data.Id,
                    $"Product with ID {productCreateResult.Data.Id} has been created by {auditStampCreateModel.Username}.",
                    auditStampCreateModel,
                    cancellationToken);

            if (auditLogCreateResult.IsFailureAndNoData)
            {
                // Just we need logging here.
            }

            return productCreateResult;
        }, cancellationToken);
    }
}

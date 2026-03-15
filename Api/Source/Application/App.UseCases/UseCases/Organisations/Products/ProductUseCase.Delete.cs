using Core.Domain.Entities.Auditing.AuditLogs;
using Core.Domain.ValueObjects.AuditStampValueObject;
using Core.Library.ResultPattern;

namespace App.UseCases.UseCases.Organisations.Products;

internal sealed partial class ProductUseCase
{
    public async Task<Result> DeleteCompanyProductAsync(
        Guid tenantId,
        Guid companyId,
        Guid productId,
        AuditStampCreateModel auditStampCreateModel,
        CancellationToken cancellationToken = default)
    {
        return await _unitOfWork.ExecuteTransactionAsync(async () =>
        {
            // Delete product.
            Result productDeleteResult = await _productService
                .DeleteCompanyProductAsync(tenantId, companyId, productId, cancellationToken);

            if (productDeleteResult.IsFailure)
                return productDeleteResult;

            // Create audit log.
            Result<AuditLog> auditLogCreateResult = await _auditLogService
                .CreateAsync(
                    productId,
                    $"Product with ID {productId} has been deleted by {auditStampCreateModel.Username}.",
                    auditStampCreateModel,
                    cancellationToken);

            if (auditLogCreateResult.IsFailureAndNoData)
            {
                // Just we need logging here.
            }

            return productDeleteResult;
        }, cancellationToken);
    }
}

using Core.Domain.Entities.Organisations.Products.Models;
using Core.Domain.ValueObjects.AuditStampValueObject;
using Core.Library.ResultPattern;

namespace Core.Domain.Entities.Organisations.Products.Interfaces;

public interface IProductUseCase
{
    /// <summary>
    /// Retrieves a paginated collection of products based on the provided parameters.
    /// </summary>
    /// <param name="tenantId">The tenant ID.</param>
    /// <param name="pageQuery">The page number.</param>
    /// <param name="pageSizeQuery">The page size.</param>
    /// <param name="sortByQuery">The field to sort by.</param>
    /// <param name="isAscendingQuery">Indicates whether the sorting should be in ascending order.</param>
    /// <param name="filterQuery">The filter criteria.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A paginated result containing the product entities.</returns>
    Task<PaginatedResult<Product[]>> GetPaginatedAsync(
        Guid tenantId,
        int? pageQuery,
        int? pageSizeQuery,
        string? sortByQuery,
        bool? isAscendingQuery,
        string? filterQuery,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a product by its ID.
    /// </summary>
    /// <param name="tenantId">The tenant ID.</param>
    /// <param name="id">The product ID.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A result containing the product if found, or an error if not.</returns>
    Task<Result<Product>> GetByIdAsync(
        Guid tenantId,
        Guid id,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a new product entity.
    /// </summary>
    /// <param name="createModel">The product create model.</param>
    /// <param name="auditStampCreateModel">The audit stamp create model.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A result containing the created product if successful, or an error if not.</returns>
    Task<Result<Product>> CreateAsync(
        ProductCreateModel createModel,
        AuditStampCreateModel auditStampCreateModel,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing product entity.
    /// </summary>
    /// <param name="tenantId">The tenant ID.</param>
    /// <param name="updateModel">The product update model.</param>
    /// <param name="auditStampCreateModel">The audit stamp create model.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A result containing the updated product if successful, or an error if not.</returns>
    Task<Result<Product>> UpdateAsync(
        Guid tenantId,
        ProductUpdateModel updateModel,
        AuditStampCreateModel auditStampCreateModel,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes an existing product entity.
    /// </summary>
    /// <param name="tenantId">The tenant ID.</param>
    /// <param name="id">The product ID.</param>
    /// <param name="auditStampCreateModel">The audit stamp create model.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A result indicating the success or failure of the delete operation.</returns>
    Task<Result> DeleteAsync(
        Guid tenantId,
        Guid id,
        AuditStampCreateModel auditStampCreateModel,
        CancellationToken cancellationToken = default);
}

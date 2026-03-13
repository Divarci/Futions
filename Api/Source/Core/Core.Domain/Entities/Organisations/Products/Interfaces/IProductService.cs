using Core.Domain.Entities.Organisations.Products.Models;
using Core.Library.ResultPattern;

namespace Core.Domain.Entities.Organisations.Products.Interfaces;

public interface IProductService
{
    /// <summary>
    /// Retrieves a paginated list of products according to the specified parameters.
    /// </summary>
    /// <param name="tenantId">The tenant ID.</param>
    /// <param name="page">The page number (1-based).</param>
    /// <param name="pageSize">The number of items per page.</param>
    /// <param name="sortBy">The field to sort by.</param>
    /// <param name="isAscending">Sort direction: true for ascending, false for descending.</param>
    /// <param name="filterQuery">Optional filter string.</param>
    /// <param name="mapper">A function to map the product entities to the desired DTO type.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A result containing the paginated array of products.</returns>
    Task<PaginatedResult<TDto[]>> GetPaginatedAsync<TDto>(
        Guid tenantId,
        int page,
        int pageSize,
        string sortBy,
        bool isAscending,
        string? filterQuery,
        Func<Product[], TDto[]> mapper,
        CancellationToken cancellationToken = default) where TDto : class;

    /// <summary>
    /// Retrieves a product by its unique identifier.
    /// </summary>
    /// <param name="tenantId">The tenant ID.</param>
    /// <param name="id">The product ID.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A result containing the product if found.</returns>
    Task<Result<Product>> GetByIdAsync(
        Guid tenantId,
        Guid id,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a new product entity.
    /// </summary>
    /// <param name="tenantId">The tenant ID.</param>
    /// <param name="createModel">The product create model.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A result containing the created product.</returns>
    Task<Result<Product>> CreateAsync(
        Guid tenantId,
        ProductCreateModel createModel,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing product entity.
    /// </summary>
    /// <param name="tenantId">The tenant ID.</param>
    /// <param name="updateModel">The product update model.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A result containing the updated product.</returns>
    Task<Result<Product>> UpdateAsync(
        Guid tenantId,
        ProductUpdateModel updateModel,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a product entity.
    /// </summary>
    /// <param name="tenantId">The tenant ID.</param>
    /// <param name="id">The product ID.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A result indicating the outcome of the delete operation.</returns>
    Task<Result> DeleteAsync(
        Guid tenantId,
        Guid id,
        CancellationToken cancellationToken = default);
}

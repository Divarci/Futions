using Core.Domain.Entities.Organisations.Products.Models;
using Core.Library.ResultPattern;

namespace Core.Domain.Entities.Organisations.Products.Interfaces;

public interface IProductService
{
    /// <summary>
    /// Retrieves a paginated list of products according to the specified parameters.
    /// </summary>
    /// <param name="page">The page number (1-based).</param>
    /// <param name="pageSize">The number of items per page.</param>
    /// <param name="sortBy">The field to sort by.</param>
    /// <param name="isAscending">Sort direction: true for ascending, false for descending.</param>
    /// <param name="filter">Optional filter string.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A result containing the paginated array of products.</returns>
    Task<Result<Product[]>> GetPaginatedAsync(
        int page,
        int pageSize,
        string sortBy,
        bool isAscending,
        string? filter,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a product by its unique identifier.
    /// </summary>
    /// <param name="id">The product ID.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A result containing the product if found.</returns>
    Task<Result<Product>> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a new product entity.
    /// </summary>
    /// <param name="createModel">The product create model.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A result containing the created product.</returns>
    Task<Result<Product>> CreateAsync(
        ProductCreateModel createModel,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing product entity.
    /// </summary>
    /// <param name="updateModel">The product update model.</param>
    /// <returns>A result containing the updated product.</returns>
    Result<Product> Update(ProductUpdateModel updateModel);

    /// <summary>
    /// Deletes a product entity.
    /// </summary>
    /// <param name="id">The product ID.</param>
    /// <returns>A result indicating the outcome of the delete operation.</returns>
    Result Delete(Guid id);

    /// <summary>
    /// Returns the total number of products.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A result containing the total count of products.</returns>
    Task<Result<int>> CountAsync(CancellationToken cancellationToken = default);
}

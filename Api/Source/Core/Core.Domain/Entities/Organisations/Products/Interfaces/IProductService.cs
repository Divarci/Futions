using Core.Domain.Entities.Organisations.Products.Models;
using Core.Library.ResultPattern;

namespace Core.Domain.Entities.Organisations.Products.Interfaces;

public interface IProductService
{
    /// <summary>
    /// Retrieves a paginated list of products according to the specified parameters.
    /// </summary>
    /// <param name="tenantId">The tenant ID.</param>
    /// <param name="companyId">The company ID.</param>
    /// <param name="page">The page number (1-based).</param>
    /// <param name="pageSize">The number of items per page.</param>
    /// <param name="sortBy">The field to sort by.</param>
    /// <param name="isAscending">Sort direction: true for ascending, false for descending.</param>
    /// <param name="filterQuery">Optional filter string.</param>
    /// <param name="mapper">Mapper function</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A result containing the paginated array of products.</returns>
    Task<PaginatedResult<TDto[]>> GetPaginatedCompanyProductsAsync<TDto>(
        Guid tenantId,
        Guid companyId,
        int page,
        int pageSize,
        string sortBy,
        bool isAscending,
        string? filterQuery,
        Func<Product[], TDto[]> mapper,
        CancellationToken cancellationToken = default) where TDto : class;

    /// <summary>
    /// Retrieves a product by its ID.
    /// </summary>
    /// <param name="tenantId">The tenant ID.</param>
    /// <param name="companyId">The company ID.</param>
    /// <param name="productId">The product ID.</param>
    /// <param name="mapper">Mapper function</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A result containing the product if found, or an error if not.</returns>
    Task<Result<TDto>> GetCompanyProductByIdAsync<TDto>(
        Guid tenantId,
        Guid companyId,
        Guid productId,
        Func<Product, TDto> mapper,
        CancellationToken cancellationToken = default) where TDto : class;

    /// <summary>
    /// Creates a new product entity.
    /// </summary>
    /// <param name="createModel">The product create model.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A result containing the created product.</returns>
    Task<Result<Product>> CreateCompanyProductAsync(
        ProductCreateModel createModel,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing product entity.
    /// </summary>
    /// <param name="updateModel">The product update model.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A result containing the updated product.</returns>
    Task<Result<Product>> UpdateCompanyProductAsync(
        ProductUpdateModel updateModel,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a product entity.
    /// </summary>
    /// <param name="tenantId">The tenant ID.</param>
    /// <param name="companyId">The company ID.</param>
    /// <param name="productId">The product ID.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A result indicating the outcome of the delete operation.</returns>
    Task<Result> DeleteCompanyProductAsync(
        Guid tenantId,
        Guid companyId,
        Guid productId,
        CancellationToken cancellationToken = default);
}

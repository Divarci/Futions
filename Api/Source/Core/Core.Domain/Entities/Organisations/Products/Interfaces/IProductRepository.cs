using Core.Domain.Entities.Organisations.Products.Models;
using Core.Library.Contracts.GenericRepositories;
using Core.Library.ResultPattern;

namespace Core.Domain.Entities.Organisations.Products.Interfaces;

public interface IProductRepository : ITenantedRepository<Product>
{
    /// <summary>
    /// Retrieves a paginated list of company products according to the specified parameters.
    /// </summary>
    /// <param name="tenantId">The tenant ID.</param>
    /// <param name="companyId">The company ID.</param>
    /// <param name="page">The page number (1-based).</param>
    /// <param name="pageSize">The number of items per page.</param>
    /// <param name="sortBy">The field to sort by.</param>
    /// <param name="isAscending">Sort direction: true for ascending, false for descending.</param>
    /// <param name="filter">Optional filter string.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A result containing the paginated array of products.</returns>
    Task<Result<Product[]>> GetPaginatedCompanyProductsAsync(
        Guid tenantId,
        Guid companyId,
        int page,
        int pageSize,
        string sortBy,
        bool isAscending,
        string? filter,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a company product by its ID.
    /// </summary>
    /// <param name="tenantId">The tenant ID.</param>
    /// <param name="companyId">The company ID.</param>
    /// <param name="productId">The product ID.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A result containing the product.</returns>
    Task<Result<Product>> GetCompanyProductByIdAsync(
        Guid tenantId,
        Guid companyId,
        Guid productId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Counts the number of products for a specific company.
    /// </summary>
    /// <param name="tenantId">The tenant ID.</param>
    /// <param name="companyId">The company ID.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A result containing the count of products.</returns>
    Task<Result<int>> CountCompanyProductsAsync(
        Guid tenantId,
        Guid companyId,
        CancellationToken cancellationToken = default);

}
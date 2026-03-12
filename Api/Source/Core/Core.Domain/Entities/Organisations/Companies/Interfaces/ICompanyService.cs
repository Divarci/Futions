using Core.Domain.Entities.Organisations.Companies.Models;
using Core.Library.ResultPattern;

namespace Core.Domain.Entities.Organisations.Companies.Interfaces;

public interface ICompanyService
{
    /// <summary>
    /// Retrieves a paginated list of companies according to the specified parameters.
    /// </summary>
    /// <param name="tenantId">The tenant ID.</param>
    /// <param name="pageQuery">The page number (1-based).</param>
    /// <param name="pageSizeQuery">The number of items per page.</param>
    /// <param name="sortByQuery">The field to sort by.</param>
    /// <param name="isAscendingQuery">Sort direction: true for ascending, false for descending.</param>
    /// <param name="filterQuery">Optional filter string.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A result containing the paginated array of companies.</returns>
    Task<Result<Company[]>> GetPaginatedAsync(
        Guid tenantId,
        int? pageQuery,
        int? pageSizeQuery,
        string? sortByQuery,
        bool? isAscendingQuery,
        string? filterQuery,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a company by its unique identifier.
    /// </summary>
    /// <param name="tenantId">The tenant ID.</param>
    /// <param name="id">The company ID.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A result containing the company if found.</returns>
    Task<Result<Company>> GetByIdAsync(
        Guid tenantId,
        Guid id,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a new company entity.
    /// </summary>
    /// <param name="tenantId">The tenant ID.</param>
    /// <param name="createModel">The company create model.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A result containing the created company.</returns>
    Task<Result<Company>> CreateAsync(
        Guid tenantId,
        CompanyCreateModel createModel,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing company entity.
    /// </summary>
    /// <param name="tenantId">The tenant ID.</param>
    /// <param name="updateModel">The company update model.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A result indicating the outcome of the update operation.</returns>
    Task<Result> UpdateAsync(
        Guid tenantId,
        CompanyUpdateModel updateModel,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a company entity.
    /// </summary>
    /// <param name="tenantId">The tenant ID.</param>
    /// <param name="id">The company ID.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A result indicating the outcome of the delete operation.</returns>
    Task<Result> DeleteAsync(
        Guid tenantId,
        Guid id,
        CancellationToken cancellationToken = default);      
}
using Core.Domain.Entities.Organisations.CompanyPeople.Models;
using Core.Library.ResultPattern;

namespace Core.Domain.Entities.Organisations.CompanyPeople.Interfaces;

public interface ICompanyPersonService
{
    /// <summary>
    /// Retrieves a paginated list of company people according to the specified parameters.
    /// </summary>
    /// <param name="tenantId">The tenant ID.</param>
    /// <param name="pageQuery">The page number (1-based).</param>
    /// <param name="pageSizeQuery">The number of items per page.</param>
    /// <param name="sortByQuery">The field to sort by.</param>
    /// <param name="isAscendingQuery">Sort direction: true for ascending, false for descending.</param>
    /// <param name="filterQuery">Optional filter string.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A result containing the paginated array of company people.</returns>
    Task<Result<CompanyPerson[]>> GetPaginatedAsync(
        Guid tenantId,
        int? pageQuery,
        int? pageSizeQuery,
        string? sortByQuery,
        bool? isAscendingQuery,
        string? filterQuery,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a company person by its unique identifier.
    /// </summary>
    /// <param name="tenantId">The tenant ID.</param>
    /// <param name="id">The company person ID.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A result containing the company person if found.</returns>
    Task<Result<CompanyPerson>> GetByIdAsync(
        Guid tenantId,
        Guid id,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a new company person entity.
    /// </summary>
    /// <param name="tenantId">The tenant ID.</param>
    /// <param name="createModel">The company person create model.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A result containing the created company person.</returns>
    Task<Result<CompanyPerson>> CreateAsync(
        Guid tenantId,
        CompanyPersonCreateModel createModel,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing company person entity.
    /// </summary>
    /// <param name="tenantId">The tenant ID.</param>
    /// <param name="updateModel">The company person update model.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A result indicating the outcome of the update operation.</returns>
    Task<Result> UpdateAsync(
        Guid tenantId,
        CompanyPersonUpdateModel updateModel,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Hard deletes a company person entity.
    /// </summary>
    /// <param name="tenantId">The tenant ID.</param>
    /// <param name="id">The company person ID.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A result indicating the outcome of the delete operation.</returns>
    Task<Result> DeleteAsync(
        Guid tenantId,
        Guid id,
        CancellationToken cancellationToken = default);
}

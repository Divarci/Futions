using Core.Domain.Entities.Organisations.CompanyPeople.Models;
using Core.Library.ResultPattern;

namespace Core.Domain.Entities.Organisations.CompanyPeople.Interfaces;

public interface ICompanyPersonService
{
    /// <summary>
    /// Retrieves a paginated list of company people according to the specified parameters.
    /// </summary>
    /// <param name="page">The page number (1-based).</param>
    /// <param name="pageSize">The number of items per page.</param>
    /// <param name="sortBy">The field to sort by.</param>
    /// <param name="isAscending">Sort direction: true for ascending, false for descending.</param>
    /// <param name="filter">Optional filter string.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A result containing the paginated array of company people.</returns>
    Task<Result<CompanyPerson[]>> GetPaginatedAsync(
        int page,
        int pageSize,
        string sortBy,
        bool isAscending,
        string? filter,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a company person by its unique identifier.
    /// </summary>
    /// <param name="id">The company person ID.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A result containing the company person if found.</returns>
    Task<Result<CompanyPerson>> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a new company person entity.
    /// </summary>
    /// <param name="createModel">The company person create model.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A result containing the created company person.</returns>
    Task<Result<CompanyPerson>> CreateAsync(
        CompanyPersonCreateModel createModel,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing company person entity.
    /// </summary>
    /// <param name="updateModel">The company person update model.</param>
    /// <returns>A result containing the updated company person.</returns>
    Result<CompanyPerson> Update(CompanyPersonUpdateModel updateModel);

    /// <summary>
    /// Hard deletes a company person entity.
    /// </summary>
    /// <param name="id">The company person ID.</param>
    /// <returns>A result indicating the outcome of the delete operation.</returns>
    Result Delete(Guid id);

    /// <summary>
    /// Returns the total number of company people.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A result containing the total count of company people.</returns>
    Task<Result<int>> CountAsync(CancellationToken cancellationToken = default);
}

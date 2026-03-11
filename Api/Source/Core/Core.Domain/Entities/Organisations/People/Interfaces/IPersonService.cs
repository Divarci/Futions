using Core.Domain.Entities.Organisations.People.Models;
using Core.Library.ResultPattern;

namespace Core.Domain.Entities.Organisations.People.Interfaces;

public interface IPersonService
{
    /// <summary>
    /// Retrieves a paginated list of people according to the specified parameters.
    /// </summary>
    /// <param name="tenantId">The tenant ID.</param>
    /// <param name="page">The page number (1-based).</param>
    /// <param name="pageSize">The number of items per page.</param>
    /// <param name="sortBy">The field to sort by.</param>
    /// <param name="isAscending">Sort direction: true for ascending, false for descending.</param>
    /// <param name="filter">Optional filter string.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A result containing the paginated array of people.</returns>
    Task<Result<Person[]>> GetPaginatedAsync(
        Guid tenantId,
        int page,
        int pageSize,
        string sortBy,
        bool isAscending,
        string? filter,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a person by its unique identifier.
    /// </summary>
    /// <param name="tenantId">The tenant ID.</param>
    /// <param name="id">The person ID.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A result containing the person if found.</returns>
    Task<Result<Person>> GetByIdAsync(
        Guid tenantId,
        Guid id,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a new person entity.
    /// </summary>
    /// <param name="tenantId">The tenant ID.</param>
    /// <param name="createModel">The person create model.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A result containing the created person.</returns>
    Task<Result<Person>> CreateAsync(
        Guid tenantId,
        PersonCreateModel createModel,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing person entity.
    /// </summary>
    /// <param name="tenantId">The tenant ID.</param>
    /// <param name="updateModel">The person update model.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A result indicating the outcome of the update operation.</returns>
    Task<Result> UpdateAsync(
        Guid tenantId,
        PersonUpdateModel updateModel,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a person entity.
    /// </summary>
    /// <param name="tenantId">The tenant ID.</param>
    /// <param name="id">The person ID.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A result indicating the outcome of the delete operation.</returns>
    Task<Result> DeleteAsync(
        Guid tenantId,
        Guid id,
        CancellationToken cancellationToken = default);
}

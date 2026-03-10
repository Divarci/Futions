using Core.Domain.Entities.Organisations.People.Models;
using Core.Library.ResultPattern;

namespace Core.Domain.Entities.Organisations.People.Interfaces;

public interface IPersonService
{
    /// <summary>
    /// Retrieves a paginated list of people according to the specified parameters.
    /// </summary>
    /// <param name="page">The page number (1-based).</param>
    /// <param name="pageSize">The number of items per page.</param>
    /// <param name="sortBy">The field to sort by.</param>
    /// <param name="isAscending">Sort direction: true for ascending, false for descending.</param>
    /// <param name="filter">Optional filter string.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A result containing the paginated array of people.</returns>
    Task<Result<Person[]>> GetPaginatedAsync(
        int page,
        int pageSize,
        string sortBy,
        bool isAscending,
        string? filter,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a person by its unique identifier.
    /// </summary>
    /// <param name="id">The person ID.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A result containing the person if found.</returns>
    Task<Result<Person>> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a new person entity.
    /// </summary>
    /// <param name="createModel">The person create model.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A result containing the created person.</returns>
    Task<Result<Person>> CreateAsync(
        PersonCreateModel createModel,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing person entity.
    /// </summary>
    /// <param name="updateModel">The person update model.</param>
    /// <returns>A result containing the updated person.</returns>
    Result<Person> Update(PersonUpdateModel updateModel);

    /// <summary>
    /// Deletes a person entity.
    /// </summary>
    /// <param name="id">The person ID.</param>
    /// <returns>A result indicating the outcome of the delete operation.</returns>
    Result Delete(Guid id);

    /// <summary>
    /// Returns the total number of people.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A result containing the total count of people.</returns>
    Task<Result<int>> CountAsync(CancellationToken cancellationToken = default);
}

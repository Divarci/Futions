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
    /// <param name="filterQuery">Optional filter string.</param>
    /// <param name="mapper">A function to map the person entities to the desired DTO type.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A result containing the paginated array of people.</returns>
    Task<PaginatedResult<TDto[]>> GetPaginatedPeopleAsync<TDto>(
        Guid tenantId,
        int page,
        int pageSize,
        string sortBy,
        bool isAscending,
        string? filterQuery,
        Func<Person[], TDto[]> mapper,
        CancellationToken cancellationToken = default) where TDto : class;

    /// <summary>
    /// Retrieves a person by its unique identifier.
    /// </summary>
    /// <param name="tenantId">The tenant ID.</param>
    /// <param name="personId">The person ID.</param>
    /// <param name="mapper">A function to map the person entity to the desired DTO type.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A result containing the person if found.</returns>
    Task<Result<TDto>> GetPersonByIdAsync<TDto>(
        Guid tenantId,
        Guid personId,
        Func<Person, TDto> mapper,
        CancellationToken cancellationToken = default) where TDto : class;

    /// <summary>
    /// Creates a new person entity.
    /// </summary>
    /// <param name="createModel">The person create model.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A result containing the created person.</returns>
    Task<Result<Person>> CreatePersonAsync(
        PersonCreateModel createModel,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing person entity.
    /// </summary>
    /// <param name="updateModel">The person update model.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A result indicating the outcome of the update operation.</returns>
    Task<Result> UpdatePersonAsync(
        PersonUpdateModel updateModel,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a person entity.
    /// </summary>
    /// <param name="tenantId">The tenant ID.</param>
    /// <param name="personId">The person ID.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A result indicating the outcome of the delete operation.</returns>
    Task<Result> DeletePersonAsync(
        Guid tenantId,
        Guid personId,
        CancellationToken cancellationToken = default);
}

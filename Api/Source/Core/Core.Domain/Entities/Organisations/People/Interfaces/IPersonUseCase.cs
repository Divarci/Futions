using Core.Domain.Entities.Organisations.People.Models;
using Core.Domain.ValueObjects.AuditStampValueObject;
using Core.Library.ResultPattern;

namespace Core.Domain.Entities.Organisations.People.Interfaces;

public interface IPersonUseCase
{
    /// <summary>
    /// Retrieves a paginated collection of people based on the provided parameters.
    /// </summary>
    /// <param name="tenantId">The tenant ID.</param>
    /// <param name="pageQuery">The page number.</param>
    /// <param name="pageSizeQuery">The page size.</param>
    /// <param name="sortByQuery">The field to sort by.</param>
    /// <param name="isAscendingQuery">Indicates whether the sorting should be in ascending order.</param>
    /// <param name="filterQuery">The filter criteria.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A paginated result containing the person entities.</returns>
    Task<PaginatedResult<Person[]>> GetPaginatedAsync(
        Guid tenantId,
        int? pageQuery,
        int? pageSizeQuery,
        string? sortByQuery,
        bool? isAscendingQuery,
        string? filterQuery,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a person by its ID.
    /// </summary>
    /// <param name="tenantId">The tenant ID.</param>
    /// <param name="id">The person ID.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A result containing the person if found, or an error if not.</returns>
    Task<Result<Person>> GetByIdAsync(
        Guid tenantId,
        Guid id,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a new person entity.
    /// </summary>
    /// <param name="createModel">The person create model.</param>
    /// <param name="auditStampCreateModel">The audit stamp create model.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A result containing the created person if successful, or an error if not.</returns>
    Task<Result<Person>> CreateAsync(
        PersonCreateModel createModel,
        AuditStampCreateModel auditStampCreateModel,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing person entity.
    /// </summary>
    /// <param name="tenantId">The tenant ID.</param>
    /// <param name="updateModel">The person update model.</param>
    /// <param name="auditStampCreateModel">The audit stamp create model.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A result indicating the success or failure of the update operation.</returns>
    Task<Result> UpdateAsync(
        Guid tenantId,
        PersonUpdateModel updateModel,
        AuditStampCreateModel auditStampCreateModel,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes an existing person entity.
    /// </summary>
    /// <param name="tenantId">The tenant ID.</param>
    /// <param name="id">The person ID.</param>
    /// <param name="auditStampCreateModel">The audit stamp create model.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A result indicating the success or failure of the delete operation.</returns>
    Task<Result> DeleteAsync(
        Guid tenantId,
        Guid id,
        AuditStampCreateModel auditStampCreateModel,
        CancellationToken cancellationToken = default);
}

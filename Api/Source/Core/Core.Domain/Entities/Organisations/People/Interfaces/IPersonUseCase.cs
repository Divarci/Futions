using Core.Domain.Entities.Organisations.People.Models;
using Core.Domain.Entities.System.AuditLogs.Models;
using Core.Library.ResultPattern;

namespace Core.Domain.Entities.Organisations.People.Interfaces;

public interface IPersonUseCase
{
    /// <summary>
    /// Retrieves a paginated collection of people based on the provided parameters.
    /// </summary>
    Task<PaginatedResult<TDto[]>> GetPaginatedAsync<TDto>(
        Guid tenantId,
        int? pageQuery,
        int? pageSizeQuery,
        string? sortByQuery,
        bool? isAscendingQuery,
        string? filterQuery,
        Func<Person[], TDto[]> mapper,
        CancellationToken cancellationToken = default) where TDto : class;

    /// <summary>
    /// Retrieves a person by its ID.
    /// </summary>
    Task<Result<Person>> GetByIdAsync(
        Guid tenantId,
        Guid id,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a new person entity.
    /// </summary>
    Task<Result<Person>> CreateAsync(
        Guid tenantId,
        PersonCreateModel createModel,
        AuditLogCreateModel auditLogCreateModel,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing person entity.
    /// </summary>
    Task<Result> UpdateAsync(
        Guid tenantId,
        PersonUpdateModel updateModel,
        AuditLogCreateModel auditLogCreateModel,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes an existing person entity.
    /// </summary>
    Task<Result> DeleteAsync(
        Guid tenantId,
        Guid id,
        AuditLogCreateModel auditLogCreateModel,
        CancellationToken cancellationToken = default);
}

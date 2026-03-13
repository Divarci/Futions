using Core.Domain.Entities.Organisations.Companies.Models;
using Core.Domain.Entities.System.AuditLogs.Models;
using Core.Library.ResultPattern;

namespace Core.Domain.Entities.Organisations.Companies.Interfaces;

public interface ICompanyUseCase
{    
    /// <summary>
    /// Retrieves a paginated collection of companies based on the provided parameters.
    /// </summary>
    /// <typeparam name="TDto">The type of the data transfer object.</typeparam>
    /// <param name="tenantId">The tenant ID.</param>
    /// <param name="pageQuery">The page number.</param>
    /// <param name="pageSizeQuery">The page size.</param>
    /// <param name="sortByQuery">The field to sort by.</param>
    /// <param name="isAscendingQuery">Indicates whether the sorting should be in ascending order.</param>
    /// <param name="filterQuery">The filter criteria.</param>
    /// <param name="mapper">A function to map the company entities to the desired DTO type.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A paginated result containing the mapped DTOs.</returns>
    Task<PaginatedResult<TDto[]>> GetPaginatedAsync<TDto>(
        Guid tenantId,
        int? pageQuery,
        int? pageSizeQuery,
        string? sortByQuery,
        bool? isAscendingQuery,
        string? filterQuery,
        Func<Company[], TDto[]> mapper,
        CancellationToken cancellationToken = default) where TDto : class;
      
    /// <summary>
    /// Retrieves a company by its ID.
    /// </summary>
    /// <param name="tenantId">The tenant ID.</param>
    /// <param name="id">The company ID.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A result containing the company if found, or an error if not.</returns>
    Task<Result<Company>> GetByIdAsync(
        Guid tenantId,
        Guid id,
        CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Creates a new company entity.
    /// </summary>
    /// <param name="tenantId">The tenant ID.</param>
    /// <param name="createModel">The company create model.</param>
    /// <param name="auditLogCreateModel">The audit log create model.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A result containing the created company if successful, or an error if not.</returns>
    Task<Result<Company>> CreateAsync(
        Guid tenantId,
        CompanyCreateModel createModel,
        AuditLogCreateModel auditLogCreateModel,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing company entity.
    /// </summary>
    /// <param name="tenantId">The tenant ID.</param>
    /// <param name="updateModel">The company update model.</param>
    /// <param name="auditLogCreateModel">The audit log create model.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A result indicating the success or failure of the update operation.</returns>
    Task<Result> UpdateAsync(
        Guid tenantId,
        CompanyUpdateModel updateModel,
        AuditLogCreateModel auditLogCreateModel,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes an existing company entity.
    /// </summary>
    /// <param name="tenantId">The tenant ID.</param>
    /// <param name="id">The company ID.</param>
    /// <param name="auditLogCreateModel">The audit log create model.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A result indicating the success or failure of the delete operation.</returns>
    Task<Result> DeleteAsync(
        Guid tenantId,
        Guid id,
        AuditLogCreateModel auditLogCreateModel,
        CancellationToken cancellationToken = default);

}

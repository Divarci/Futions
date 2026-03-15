using Core.Domain.Entities.Organisations.Companies.Models;
using Core.Domain.ValueObjects.AuditStampValueObject;
using Core.Library.ResultPattern;

namespace Core.Domain.Entities.Organisations.Companies.Interfaces;

public interface ICompanyUseCase
{    
    /// <summary>
    /// Retrieves a paginated collection of companies based on the provided parameters.
    /// </summary>
    /// <param name="tenantId">The tenant ID.</param>
    /// <param name="pageQuery">The page number.</param>
    /// <param name="pageSizeQuery">The page size.</param>
    /// <param name="sortByQuery">The field to sort by.</param>
    /// <param name="isAscendingQuery">Indicates whether the sorting should be in ascending order.</param>
    /// <param name="filterQuery">The filter criteria.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A paginated result containing the company entities.</returns>
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
    Task<Result<TDto>> GetByIdAsync<TDto>(
        Guid tenantId,
        Guid id,
        Func<Company, TDto> mapper,
        CancellationToken cancellationToken = default) where TDto : class;
    
    /// <summary>
    /// Creates a new company entity.
    /// </summary>
    /// <param name="createModel">The company create model.</param>
    /// <param name="auditStampCreateModel">The audit stamp create model.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A result containing the created company if successful, or an error if not.</returns>
    Task<Result<Company>> CreateAsync(
        CompanyCreateModel createModel,
        AuditStampCreateModel auditStampCreateModel,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing company entity.
    /// </summary>
    /// <param name="tenantId">The tenant ID.</param>
    /// <param name="updateModel">The company update model.</param>
    /// <param name="auditStampCreateModel">The audit stamp create model.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A result indicating the success or failure of the update operation.</returns>
    Task<Result> UpdateAsync(
        Guid tenantId,
        CompanyUpdateModel updateModel,
        AuditStampCreateModel auditStampCreateModel,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes an existing company entity.
    /// </summary>
    /// <param name="tenantId">The tenant ID.</param>
    /// <param name="id">The company ID.</param>
    /// <param name="auditStampCreateModel">The audit stamp create model.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A result indicating the success or failure of the delete operation.</returns>
    Task<Result> DeleteAsync(
        Guid tenantId,
        Guid id,
        AuditStampCreateModel auditStampCreateModel,
        CancellationToken cancellationToken = default);

}

using Core.Domain.Entities.Organisations.Companies.Models;
using Core.Library.ResultPattern;

namespace Core.Domain.Entities.Organisations.Companies.Interfaces;

public interface ICompanyService
{
    /// <summary>
    /// Retrieves a paginated list of companies according to the specified parameters.
    /// </summary>
    /// <param name="tenantId">The tenant ID.</param>
    /// <param name="page">The page number (1-based).</param>
    /// <param name="pageSize">The number of items per page.</param>
    /// <param name="sortBy">The field to sort by.</param>
    /// <param name="isAscending">Sort direction: true for ascending, false for descending.</param>
    /// <param name="filterQuery">Optional filter string.</param>
    /// <param name="mapper">A function to map the company entities to the desired DTO type.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A result containing the paginated array of companies.</returns>
    Task<PaginatedResult<TDto[]>> GetPaginatedCompaniesAsync<TDto>(
        Guid tenantId,
        int page,
        int pageSize,
        string sortBy,
        bool isAscending,
        string? filterQuery,
        Func<Company[], TDto[]> mapper,
        CancellationToken cancellationToken = default) where TDto : class;

    /// <summary>
    /// Retrieves a company by its unique identifier.
    /// </summary>
    /// <param name="tenantId">The tenant ID.</param>
    /// <param name="companyId">The company ID.</param>
    /// <param name="mapper">A function to map the company entity to the desired DTO type.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A result containing the company if found.</returns>
    Task<Result<TDto>> GetCompanyByIdAsync<TDto>(
        Guid tenantId,
        Guid companyId,
        Func<Company, TDto> mapper,
        CancellationToken cancellationToken = default) where TDto : class;

    /// <summary>
    /// Creates a new company entity.
    /// </summary>
    /// <param name="createModel">The company create model.</param>
    /// <param name="cacheKeyBuilder">A function that builds the cache key for the affected entity.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A result containing the created company.</returns>
    Task<Result<Company>> CreateCompanyAsync(
        CompanyCreateModel createModel,
        Func<string, (string Label, object Value)[], string> cacheKeyBuilder,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing company entity.
    /// </summary>
    /// <param name="updateModel">The company update model.</param>
    /// <param name="cacheKeyBuilder">A function that builds the cache key for the affected entity.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A result indicating the outcome of the update operation.</returns>
    Task<Result> UpdateCompanyAsync(
        CompanyUpdateModel updateModel,
        Func<string, (string Label, object Value)[], string> cacheKeyBuilder,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a company entity.
    /// </summary>
    /// <param name="tenantId">The tenant ID.</param>
    /// <param name="companyId">The company ID.</param>
    /// <param name="cacheKeyBuilder">A function that builds the cache key for the affected entity.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A result indicating the outcome of the delete operation.</returns>
    Task<Result> DeleteCompanyAsync(
        Guid tenantId,
        Guid companyId,
        Func<string, (string Label, object Value)[], string> cacheKeyBuilder,
        CancellationToken cancellationToken = default);
}
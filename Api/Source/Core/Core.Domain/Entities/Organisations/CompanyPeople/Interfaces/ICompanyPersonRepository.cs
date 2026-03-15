using Core.Library.Contracts.GenericRepositories;
using Core.Library.ResultPattern;

namespace Core.Domain.Entities.Organisations.CompanyPeople.Interfaces;

public interface ICompanyPersonRepository : IGlobalRepository<CompanyPerson>
{
    /// <summary>
    /// Retrieves a paginated list of company people according to the specified parameters.
    /// </summary>
    /// <param name="tenantId">The tenant ID to filter company people by.</param>
    /// <param name="companyId">The company ID to check for associated company people.</param>
    /// <param name="page">The page number (1-based).</param>
    /// <param name="pageSize">The number of items per page.</param>
    /// <param name="sortBy">The field to sort by.</param>
    /// <param name="isAscending">Sort direction: true for ascending, false for descending.</param>
    /// <param name="filter">Optional filter string.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A result containing the paginated array of company people.</returns>
    Task<Result<CompanyPerson[]>> GetPaginatedCompanyPeopleAsync(
        Guid tenantId,
        Guid companyId,
        int page,
        int pageSize,
        string sortBy,
        bool isAscending,
        string? filter,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks if a company has any associated company people.
    /// </summary>
    /// <param name="tenantId">The tenant ID to filter company people by.</param>
    /// <param name="companyId">The company ID to check for associated company people.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A result indicating whether the company has any associated company people.</returns>
    Task<Result<bool>> HasPeopleAsync(
        Guid tenantId,
        Guid companyId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks if a person has any associated companies.
    /// </summary>
    /// <param name="tenantId">The tenant ID to filter company people by.</param>
    /// <param name="personId">The person ID to check for associated companies.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A result indicating whether the person has any associated companies.</returns>
    Task<Result<bool>> HasCompanyAsync(
        Guid tenantId,
        Guid personId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks if a company person belongs to the specified tenant.
    /// </summary>
    /// <param name="tenantId">The tenant ID to check against.</param>
    /// <param name="companyId">The company ID to check for associated company people.</param>
    /// <param name="companyPersonId">The company person ID to check.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A result indicating whether the company person belongs to the specified tenant.</returns>
    Task<Result<bool>> CheckIfBelongsToTenantAsync(
        Guid tenantId,
        Guid companyId,
        Guid companyPersonId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a company person by their ID for the specified tenant.
    /// </summary>
    /// <param name="tenantId">The tenant ID.</param>
    /// <param name="companyId">The company ID.</param>
    /// <param name="companyPersonId">The company person ID.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A result containing the company person belonging to the specified tenant.</returns>
    Task<Result<CompanyPerson>> GetCompanyPersonByIdAsync(
        Guid tenantId,
        Guid companyId,
        Guid companyPersonId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Counts the number of company people for the specified tenant and company.
    /// </summary>
    /// <param name="tenantId">The tenant ID.</param>
    /// <param name="companyId">The company ID.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A result containing the count of company people.</returns>
    Task<Result<int>> CountCompanyPeopleAsync(
        Guid tenantId,
        Guid companyId,
        CancellationToken cancellationToken = default);

}

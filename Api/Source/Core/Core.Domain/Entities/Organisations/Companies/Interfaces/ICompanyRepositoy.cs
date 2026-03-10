using Core.Library.Contracts.GenericRepository;
using Core.Library.ResultPattern;

namespace Core.Domain.Entities.Organisations.Companies.Interfaces;

public interface ICompanyRepositoy : ITenantedRepository<Company>
{
    /// <summary>
    /// Retrieves a paginated list of companies according to the specified parameters.
    /// </summary>
    /// <param name="page">The page number (1-based).</param>
    /// <param name="pageSize">The number of items per page.</param>
    /// <param name="sortBy">The field to sort by.</param>
    /// <param name="isAscending">Sort direction: true for ascending, false for descending.</param>
    /// <param name="filter">Optional filter string.</param>
    /// <param name="tenantId">The tenant ID to filter companies by.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A result containing the paginated array of companies.</returns>
    Task<Result<Company[]>> GetPaginatedAsync(
        Guid tenantId,
        int page,
        int pageSize,
        string sortBy,
        bool isAscending,
        string? filter,
        CancellationToken cancellationToken = default);
}

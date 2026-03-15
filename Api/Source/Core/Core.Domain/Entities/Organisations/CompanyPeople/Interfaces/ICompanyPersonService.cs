using Core.Domain.Entities.Organisations.CompanyPeople.Models;
using Core.Library.ResultPattern;

namespace Core.Domain.Entities.Organisations.CompanyPeople.Interfaces;

public interface ICompanyPersonService
{
    /// <summary>
    /// Retrieves a paginated collection of company people based on the provided parameters.
    /// </summary>
    /// <param name="tenantId">The tenant ID.</param>
    /// <param name="companyId">The company ID.</param>
    /// <param name="pageQuery">The page number.</param>
    /// <param name="pageSizeQuery">The page size.</param>
    /// <param name="sortByQuery">The field to sort by.</param>
    /// <param name="isAscendingQuery">Indicates whether the sorting should be in ascending order.</param>
    /// <param name="filterQuery">The filter criteria.</param>
    /// <param name="mapper">Mapper function</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A paginated result containing the company person entities.</returns>
    Task<PaginatedResult<TDto[]>> GetPaginatedCompanyPeopleAsync<TDto>(
        Guid tenantId,
        Guid companyId,
        int pageQuery,
        int pageSizeQuery,
        string sortByQuery,
        bool isAscendingQuery,
        string filterQuery,
        Func<CompanyPerson[], TDto[]> mapper,
        CancellationToken cancellationToken = default) where TDto : class;

    /// <summary>
    /// Retrieves a company person by its ID.
    /// </summary>
    /// <param name="tenantId">The tenant ID.</param>
    /// <param name="companyId">The company ID.</param>
    /// <param name="companyPersonId">The company person ID.</param>
    /// <param name="mapper">Mapper function</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A result containing the company person if found, or an error if not.</returns>
    Task<Result<TDto>> GetCompanyPersonByIdAsync<TDto>(
        Guid tenantId,
        Guid companyId,
        Guid companyPersonId,
        Func<CompanyPerson, TDto> mapper,
        CancellationToken cancellationToken = default) where TDto : class;

    /// <summary>
    /// Creates a new company person entity.
    /// </summary>
    /// <param name="tenantId">The tenant ID.</param>
    /// <param name="createModel">The company person create model.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A result containing the created company person.</returns>
    Task<Result<CompanyPerson>> CreateCompanyPersonAsync(
        Guid tenantId,
        CompanyPersonCreateModel createModel,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing company person entity.
    /// </summary>
    /// <param name="updateModel">The company person update model.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A result indicating the outcome of the update operation.</returns>
    Task<Result> UpdateCompanyPersonAsync(
        CompanyPersonUpdateModel updateModel,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Hard deletes a company person entity.
    /// </summary>
    /// <param name="tenantId">The tenant ID.</param>
    /// <param name="companyId">The company ID.</param>
    /// <param name="companyPersonId">The company person ID.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A result indicating the outcome of the delete operation.</returns>
    Task<Result> DeleteCompanyPersonAsync(
        Guid tenantId,
        Guid companyId,
        Guid companyPersonId,
        CancellationToken cancellationToken = default);
}

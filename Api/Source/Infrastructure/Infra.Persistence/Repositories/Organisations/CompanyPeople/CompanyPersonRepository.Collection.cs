using Core.Domain.Entities.Organisations.CompanyPeople;
using Core.Library.ResultPattern;
using Infra.Persistence.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace Infra.Persistence.Repositories.Organisations.CompanyPeople;

internal sealed partial class CompanyPersonRepository
{
    public async Task<Result<CompanyPerson[]>> GetPaginatedCompanyPeopleAsync(
        Guid tenantId,
        Guid companyId,
        int page,
        int pageSize,
        string sortBy,
        bool isAscending,
        string? filter,
        CancellationToken cancellationToken = default)
    {
        var query = _context.Set<CompanyPerson>()
            .AsNoTracking()
            .Where(x => 
                x.Company.TenantId == tenantId && 
                x.Person.TenantId == tenantId && 
                x.CompanyId == companyId &&
                !x.Company.IsDeleted && !x.Person.IsDeleted)
            .WhereIf(!string.IsNullOrWhiteSpace(filter), x => EF.Functions.Like(x.Title, $"%{filter}%"))
            .OrderByIf(isAscending, sortBy);

        CompanyPerson[] companyPeople = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToArrayAsync(cancellationToken);

        return Result<CompanyPerson[]>.Success(
            message: "Company people retrieved successfully.",
            data: companyPeople,
            statusCode: HttpStatusCode.OK);
    }
}

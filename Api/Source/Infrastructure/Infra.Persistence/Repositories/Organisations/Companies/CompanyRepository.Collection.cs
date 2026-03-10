using Core.Domain.Entities.Organisations.Companies;
using Core.Library.ResultPattern;
using Infra.Persistence.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace Infra.Persistence.Repositories.Organisations.Companies;

internal sealed partial class CompanyRepository
{
    public async Task<Result<Company[]>> GetPaginatedAsync(
        Guid tenantId,
        int page, 
        int pageSize,
        string sortBy,
        bool isAscending,
        string? filter,
        CancellationToken cancellationToken = default)
    {
        var query = _context.Set<Company>()
            .AsNoTracking()
            .Where(x => x.TenantId == tenantId && !x.IsDeleted)
            .WhereIf(!string.IsNullOrWhiteSpace(filter), x => EF.Functions.Like(x.Name, $"%{filter}%"))
            .OrderByIf(isAscending, sortBy);

        Company[] companies = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToArrayAsync(cancellationToken);

        return Result<Company[]>.Success(
            message: "Companies retrieved successfully.",
            data: companies,
            statusCode: HttpStatusCode.OK);
    }
}

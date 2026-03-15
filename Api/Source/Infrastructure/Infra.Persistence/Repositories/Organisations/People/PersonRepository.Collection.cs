using Core.Domain.Entities.Organisations.People;
using Core.Library.ResultPattern;
using Infra.Persistence.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace Infra.Persistence.Repositories.Organisations.People;

internal sealed partial class PersonRepository
{
    public async Task<Result<Person[]>> GetPaginatedPeopleAsync(
        Guid tenantId,
        int page,
        int pageSize,
        string sortBy,
        bool isAscending,
        string? filter,
        CancellationToken cancellationToken = default)
    {
        var query = _context.Set<Person>()
            .AsNoTracking()
            .Where(x => x.TenantId == tenantId && !x.IsDeleted);

        if (!string.IsNullOrWhiteSpace(filter))
            query = query.Where(x =>
                EF.Functions.Like(x.Fullname.FirstName, $"%{filter}%") ||
                EF.Functions.Like(x.Fullname.LastName, $"%{filter}%"));

        query = query.OrderByIf(isAscending, sortBy);

        Person[] people = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToArrayAsync(cancellationToken);

        return Result<Person[]>.Success(
            message: "People retrieved successfully.",
            data: people,
            statusCode: HttpStatusCode.OK);
    }
}

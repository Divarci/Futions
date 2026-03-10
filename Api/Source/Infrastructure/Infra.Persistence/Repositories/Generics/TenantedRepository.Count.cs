using Core.Library.ResultPattern;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace Infra.Persistence.Repositories.Generics;

internal partial class TenantedRepository<TEntity>
{
    public async Task<Result<int>> CountAsync(
        Guid tenantId,
        CancellationToken cancellationToken = default)
    {
        int count = await _context
            .Set<TEntity>()
            .CountAsync(x=>x.TenantId == tenantId, cancellationToken)
            .ConfigureAwait(false);

        return Result<int>.Success(
            message: "Entity count retrieved successfully.",
            data: count,
            statusCode: HttpStatusCode.OK);
    }
}
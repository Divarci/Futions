using Core.Library.ResultPattern;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace Infra.Persistence.Repositories.Generics;

internal partial class TenantedRepository<TEntity>
{
    public async Task<Result<bool>> ExistsAsync(
        Guid id,
        Guid tenantId,
        CancellationToken cancellationToken = default)
    {
        bool exists = await _context
            .Set<TEntity>()
            .AnyAsync(e => e.Id == id && e.TenantId == tenantId, cancellationToken)
            .ConfigureAwait(false);

        if (!exists)
            return Result<bool>.Failure(
                message: $"Entity with id {id} not found.",
                statusCode: HttpStatusCode.NotFound);

        return Result<bool>.Success(
            message: $"Entity with id {id} exists.",
            data: true,
            statusCode: HttpStatusCode.OK);

    }
}
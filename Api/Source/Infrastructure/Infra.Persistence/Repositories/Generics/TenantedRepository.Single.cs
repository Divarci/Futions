using Core.Library.ResultPattern;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace Infra.Persistence.Repositories.Generics;

internal partial class TenantedRepository<TEntity> 
{
    public async Task<Result<TEntity>> GetByIdAsync(
        Guid id,
        Guid tenantId,
        CancellationToken cancellationToken = default)
    {
        TEntity? entity = await _context.Set<TEntity>()
           .SingleOrDefaultAsync(e => e.Id == id && e.TenantId == tenantId, cancellationToken)
           .ConfigureAwait(false);

        if (entity is null)
            return Result<TEntity>.Failure(
                message: $"Entity with id {id} not found.",
                statusCode: HttpStatusCode.NotFound);

        return Result<TEntity>.Success(
            message: $"Entity with id {id} retrieved successfully.",
            data: entity,
            statusCode: HttpStatusCode.OK);
    }
}
using Core.Library.Abstractions;
using Core.Library.Abstractions.Interfaces;
using Core.Library.Contracts.GenericRepository;
using Infra.Persistence.Context;

namespace Infra.Persistence.Repositories.Generics;

internal partial class TenantedRepository<TEntity>(
    AppDbContext context) : BaseRepository<TEntity>(context), ITenantedRepository<TEntity>
    where TEntity : BaseEntity, IHaveTenant
{
}

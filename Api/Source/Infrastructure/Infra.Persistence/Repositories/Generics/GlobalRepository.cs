using Core.Library.Abstractions;
using Core.Library.Contracts.GenericRepositories;
using Infra.Persistence.Context;

namespace Infra.Persistence.Repositories.Generics;

internal partial class GlobalRepository<TEntity>(
    AppDbContext context) : BaseRepository<TEntity>(context), IGlobalRepository<TEntity>
    where TEntity : BaseEntity
{
}

using Core.Library.Abstractions;
using Core.Library.Contracts.GenericRepositories;
using Infra.Persistence.Context;

namespace Infra.Persistence.Repositories.Generics;

internal abstract partial class BaseRepository<TEntity>(
    AppDbContext context) : IBaseRepository<TEntity> where TEntity : BaseEntity
{
    protected readonly AppDbContext _context = context;
}

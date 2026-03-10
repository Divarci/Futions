using Core.Library.Abstractions;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Reflection;

namespace Infra.Persistence.Extensions;

public static class IQueryableExtensions
{
    public static IQueryable<T> WhereIf<T>(
        this IQueryable<T> query, bool condition, Expression<Func<T, bool>> predicate) where T : BaseEntity
        => condition
            ? query.Where(predicate)
            : query;

    public static IQueryable<T> IncludeIf<T, TProperty>(
        this IQueryable<T> query, bool condition, Expression<Func<T, TProperty>> includeExpression) where T : BaseEntity
        => condition
            ? query.Include(includeExpression)
            : query;

    public static IOrderedQueryable<T> OrderByIf<T>(
        this IQueryable<T> query, bool isAscending, string sortBy)
    {
        var entityType = typeof(T);
        var prop = entityType.GetProperty(sortBy, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);

        if (prop is not null) sortBy = prop.Name;
        else sortBy = nameof(BaseEntity.Id);

        return isAscending
            ? query.OrderBy(x => EF.Property<object>(x!, sortBy))
            : query.OrderByDescending(x => EF.Property<object>(x!, sortBy));
    }
}

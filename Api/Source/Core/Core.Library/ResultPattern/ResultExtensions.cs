using System.Net;

namespace Core.Library.ResultPattern;

public static class ResultExtension
{
    public static Result<TModel> MapTo<TEntity, TModel>(this Result<TEntity> result, Func<TEntity, TModel> mapper)
    {
        if (result.IsFailureAndNoData)
            throw new InvalidOperationException("Cannot map a failed result.");

        return Result<TModel>.Success(
            message: result.Message,
            data: mapper(result.Data),
            statusCode: result.StatusCode);
    }

    public static PaginatedResult<TModel> MapTo<TEntity, TModel>(this PaginatedResult<TEntity> result, Func<TEntity, TModel> mapper)
    {
        if (result.IsFailureAndNoData)
            throw new InvalidOperationException("Cannot map a failed result.");

        return PaginatedResult<TModel>.Success(
            data: mapper(result.Data),
            message: result.Message,
            pageNumber: result.Metadata?.PageNumber ?? 0,
            pageSize: result.Metadata?.PageSize ?? 0,
            totalCount: result.Metadata?.TotalCount ?? 0,
            pageCount: result.Metadata?.PageCount ?? 0);
    }   
}
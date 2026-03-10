using Core.Library.Contracts.UnitOfWorks;
using Core.Library.ResultPattern;
using Infra.Persistence.Context;
using System.Net;

namespace Infra.Persistence.UnitOfWorks;

internal sealed class UnitOfWork(AppDbContext context) : IUnitOfWork, ITransactionalUnitOfWork
{
    public async Task CommitAsync(CancellationToken cancellationToken = default)
        => await context.SaveChangesAsync(cancellationToken);

    public async Task<Result> ExecuteTransactionAsync(
        Func<Task<Result>> operation,
        CancellationToken cancellationToken = default)
    {
        await using var transaction = await context.Database
            .BeginTransactionAsync(cancellationToken);

        try
        {
            Result result = await operation();

            if (result.IsFailure)
            {
                await transaction.RollbackAsync(cancellationToken);

                return result;
            }

            await transaction.CommitAsync(cancellationToken);

            return result;
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(cancellationToken);

            return Result.Failure
                (message: ex.Message, 
                statusCode: HttpStatusCode.InternalServerError);
        }
    }

    public async Task<Result<T>> ExecuteTransactionAsync<T>(
        Func<Task<Result<T>>> operation,
        CancellationToken cancellationToken = default)
    {
        await using var transaction = await context.Database
            .BeginTransactionAsync(cancellationToken);

        try
        {
            Result<T> result = await operation();

            if (result.IsFailureAndNoData)
            {
                await transaction.RollbackAsync(cancellationToken);

                return result;
            }

            await transaction.CommitAsync(cancellationToken);

            return result;
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(cancellationToken);

            return Result<T>.Failure(
                message: ex.Message, 
                statusCode: HttpStatusCode.InternalServerError);
        }
    }
}
using Core.Domain.Entities.System.OutboxMessages;
using Core.Library.Abstractions;
using Core.Library.Contracts.UnitOfWorks;
using Core.Library.ResultPattern;
using Infra.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;

namespace Infra.Persistence.UnitOfWorks;

internal sealed class UnitOfWork(AppDbContext context, ILogger<UnitOfWork> logger) : ITransactionalUnitOfWork
{ 
    public async Task<Result> ExecuteTransactionAsync(
        Func<Task<Result>> operation,
        CancellationToken cancellationToken = default)
    {
        await using var transaction = await context.Database
            .BeginTransactionAsync(cancellationToken);

        try
        {
            logger.LogDebug("Transaction started.");

            Result result = await operation();

            if (result.IsFailure)
            {
                await transaction.RollbackAsync(cancellationToken);

                logger.LogWarning(
                    "Transaction rolled back. Operation returned a failure result. {Message}",
                    result.Message);

                return result;
            }

            Result domainEventsRegisterResult = await AddDomainEventsAsOutboxMessages(context);

            if (domainEventsRegisterResult.IsFailure)
            {
                await transaction.RollbackAsync(cancellationToken);

                logger.LogError(
                    "Transaction rolled back. Failed to register domain events. {Message}",
                    domainEventsRegisterResult.Message);

                return domainEventsRegisterResult;
            }

            await context.SaveChangesAsync(cancellationToken);

            await transaction.CommitAsync(cancellationToken);

            logger.LogDebug("Transaction committed successfully.");

            return result;
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(cancellationToken);

            logger.LogError(ex, "Transaction rolled back due to an unhandled exception.");

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
            logger.LogDebug("Transaction started.");

            Result<T> result = await operation();

            if (result.IsFailureAndNoData)
            {
                await transaction.RollbackAsync(cancellationToken);

                logger.LogWarning(
                    "Transaction rolled back. Operation returned a failure result. {Message}",
                    result.Message);

                return result;
            }

            Result domainEventsRegisterResult = await AddDomainEventsAsOutboxMessages(context);

            if (domainEventsRegisterResult.IsFailure)
            {
                await transaction.RollbackAsync(cancellationToken);

                logger.LogError(
                    "Transaction rolled back. Failed to register domain events. {Message}",
                    domainEventsRegisterResult.Message);

                return Result<T>.Failure(
                    message: domainEventsRegisterResult.Message,
                    statusCode: domainEventsRegisterResult.StatusCode);
            }

            await context.SaveChangesAsync(cancellationToken);

            await transaction.CommitAsync(cancellationToken);

            logger.LogDebug("Transaction committed successfully.");

            return result;
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(cancellationToken);

            logger.LogError(ex, "Transaction rolled back due to an unhandled exception.");

            return Result<T>.Failure(
                message: ex.Message,
                statusCode: HttpStatusCode.InternalServerError);
        }
    }

    private async static Task<Result> AddDomainEventsAsOutboxMessages(DbContext dbContext)
    {
        List<Result<OutboxMessage>> outboxMessageResults = [.. dbContext.ChangeTracker
            .Entries<BaseEntity>()
            .Select(entry => entry.Entity)
            .SelectMany(entity =>
            {
                var domainEvents = entity.DomainEvents;

                entity.ClearDomainEvents();

                return domainEvents;
            })
            .Select(domainEvent => OutboxMessage.Create(
                Guid.NewGuid(),
                domainEvent.GetType().AssemblyQualifiedName!,
                JsonSerializer.Serialize(domainEvent, SerializerOptions.Instance),
                DateTime.UtcNow))];

        if (outboxMessageResults.Any(x => x.IsFailure))
            return Result.Failure(
                message: "Failed to create outbox messages from domain events.",
                statusCode: HttpStatusCode.InternalServerError);

        await dbContext.AddRangeAsync(outboxMessageResults.Select(x => x.Data)!);

        return Result.Success(
            message: "Outbox messages created successfully",
            statusCode: HttpStatusCode.OK);
    }
}
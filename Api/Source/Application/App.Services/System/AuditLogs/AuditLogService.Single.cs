using Core.Domain.Entities.Auditing.AuditLogs;
using Core.Library.ResultPattern;

namespace App.Services.Features.Organisations.Companies;

internal sealed partial class AuditLogService
{
    public async Task<Result<TDto>> GetByIdAsync<TDto>(
        Guid tenantId, 
        Guid id, 
        Func<AuditLog, TDto> mapper,
        CancellationToken cancellationToken = default) where TDto : class
    {
        // Get the AuditLog entity from the database using the repository.
        Result<AuditLog> entityResult = await _auditLogRepository
            .GetByIdAsync(tenantId, id, cancellationToken);

        if (entityResult.IsFailureAndNoData)
            return Result<TDto>.Failure(
                message: entityResult.Message,
                statusCode: entityResult.StatusCode);

        return Result<TDto>.Success(
            message: entityResult.Message,
            data: mapper(entityResult.Data),
            statusCode: entityResult.StatusCode);
    }
}
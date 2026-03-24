using Core.Domain.Entities.System.AuditLogs.Interfaces;
using Core.Library.Contracts.Caching;

namespace App.Services.Features.System.AuditLogs;

internal sealed partial class AuditLogService(
    IAuditLogRepository auditLogRepository,
    ICacheInvalidationService cacheInvalidationService) : IAuditLogService
{
    private readonly IAuditLogRepository _auditLogRepository = auditLogRepository;
    private readonly ICacheInvalidationService _cacheInvalidationService = cacheInvalidationService;
}

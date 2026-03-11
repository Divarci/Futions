using Core.Domain.Entities.System.AuditLogs.Interfaces;

namespace App.Services.Features.Organisations.Companies;

internal sealed partial class AuditLogService(
    IAuditLogRepository auditLogRepository) : IAuditLogService
{
    private readonly IAuditLogRepository _auditLogRepository = auditLogRepository;
}

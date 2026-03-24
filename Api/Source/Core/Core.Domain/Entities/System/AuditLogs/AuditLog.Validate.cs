using Core.Library.ResultPattern;
using Core.Library.Validators;

namespace Core.Domain.Entities.System.AuditLogs;

public partial class AuditLog
{
    private static Result Validate(AuditLog auditLog)
    {
        List<Result> results = [];

        ValidateProperties(results, auditLog);
        ValidateBusiness(results, auditLog);

        return Result.CombineValidationErrors(results);
    }

    private static void ValidateProperties(List<Result> results, AuditLog auditLog)
    {
        results.AddRange(
            [
                auditLog.TenantId.Validate(
                    fieldName: nameof(auditLog.TenantId),
                    allowEmpty: false),
                auditLog.EntityId.Validate(
                    fieldName: nameof(auditLog.EntityId),
                    allowEmpty: false),
                auditLog.Description.Validate(
                    fieldName: nameof(auditLog.Description),
                    maxLength: 500)
            ]
        );
    }

    private static void ValidateBusiness(List<Result> results, AuditLog auditLog)
    {
    }
}

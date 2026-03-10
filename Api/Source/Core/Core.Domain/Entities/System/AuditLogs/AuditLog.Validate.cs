using Core.Library.ResultPattern;
using Core.Library.Validators;
using System.Net;

namespace Core.Domain.Entities.Auditing.AuditLogs;

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
                    allowEmpty: false)
            ]
        );
    }

    private static void ValidateBusiness(List<Result> results, AuditLog auditLog)
    {
        if (auditLog.Updated is not null && auditLog.Updated.Timestamp < auditLog.Created.Timestamp)
            results.Add(Result.Failure(
                message: $"The {nameof(auditLog.Updated)} timestamp cannot be earlier than the {nameof(auditLog.Created)} timestamp.",
                statusCode: HttpStatusCode.BadRequest));
    }
}

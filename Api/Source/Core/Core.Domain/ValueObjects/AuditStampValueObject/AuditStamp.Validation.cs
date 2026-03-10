using Core.Library.ResultPattern;
using Core.Library.Validators;
using Core.Library.Validators.Enums;

namespace Core.Domain.ValueObjects.AuditStampValueObject;

public sealed partial record AuditStamp
{
    private static Result Validate(AuditStamp auditStamp)
    {
        List<Result> results = [];

        ValidateProperties(results, auditStamp);
        ValidateBusiness(results, auditStamp);

        return Result.CombineValidationErrors(results);
    }

    private static void ValidateProperties(List<Result> results, AuditStamp auditStamp)
        => results.AddRange(
            [
                auditStamp.Timestamp.Validate(
                    fieldName: nameof(auditStamp.Timestamp),
                    restriction: DateTimeRestriction.OnlyPast),
                auditStamp.UserId.Validate(
                    fieldName: nameof(auditStamp.UserId),
                    allowEmpty: false),
                auditStamp.Username.Validate(
                    fieldName: nameof(auditStamp.Username),
                    maxLength: 100),
            ]);

    private static void ValidateBusiness(List<Result> results, AuditStamp auditStamp)
    {
    }
}

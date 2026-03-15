using Core.Library.ResultPattern;
using System.Net;

namespace Core.Domain.ValueObjects.AuditStampValueObject;

public sealed partial record AuditStamp
{
    // Constructor
    private AuditStamp(DateTime timestamp, Guid userId, string username)
    {
        Timestamp = timestamp;
        UserId = userId;
        Username = username;
    }

    // Properties
    public DateTime Timestamp { get; private set; }
    public Guid UserId { get; private set; }
    public string Username { get; private set; }

    // Methods
    public static Result<AuditStamp> Create(AuditStampCreateModel model)
    {
        if (model is null)
            return Result<AuditStamp>.Failure(
                message: "Model can not be null",
                statusCode: HttpStatusCode.InternalServerError);

        AuditStamp auditStamp = new(
            model.Timestamp,
            model.UserId,
            model.Username);

        Result validationResult = Validate(auditStamp);

        if (validationResult.IsFailure)
            return Result<AuditStamp>.Failure(
                message: validationResult.Message,
                errorDetails: validationResult.ErrorDetails!,
                statusCode: validationResult.StatusCode);

        return Result<AuditStamp>.Success(
            message: "Audit stamp created successfully",
            data: auditStamp);
    }
}

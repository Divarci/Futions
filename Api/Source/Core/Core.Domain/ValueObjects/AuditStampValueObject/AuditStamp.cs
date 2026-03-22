using Core.Library.Exceptions;
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
    public DateTime Timestamp { get; init; }
    public Guid UserId { get; init; }
    public string Username { get; init; }

    // Methods
    public static Result<AuditStamp> Create(AuditStampCreateModel model)
    {
        if (model is null)
            throw new FutionsException(
                assemblyName: "Core.Domain",
                className: nameof(AuditStamp),
                methodName: nameof(Create),
                message: "Create model cannot be null.");

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
            data: auditStamp,
            statusCode: HttpStatusCode.OK);
    }
}

using Core.Domain.Entities.System.AuditLogs.DomainEvents;
using Core.Domain.ValueObjects.AuditStampValueObject;
using Core.Library.Abstractions;
using Core.Library.Abstractions.Interfaces;
using Core.Library.ResultPattern;
using System.Net;

namespace Core.Domain.Entities.Auditing.AuditLogs;

public partial class AuditLog : BaseEntity, IHaveTenant
{
    // Constructors
    private AuditLog() { }

    private AuditLog(string description, Guid entityId, Guid tenantId, AuditStamp created)
    {
        Description = description;
        EntityId = entityId;
        TenantId = tenantId;
        Created = created;
    }

    // Properties
    public string Description { get; private set; } = default!;
    public Guid EntityId { get; private set; }
    public AuditStamp Created { get; private set; } = default!;

    // IHaveTenant properties
    public Guid TenantId { get; private set; }

    // Methods
    public static Result<AuditLog> Create(AuditStampCreateModel model, Guid entityId, string description)
    {
        if (model is null)
            return Result<AuditLog>.Failure(
                message: "Model can not be null",
                statusCode: HttpStatusCode.InternalServerError);

        Result<AuditStamp> stampResult = AuditStamp.Create(model);

        if (stampResult.IsFailure)
            return Result<AuditLog>.Failure(
                message: stampResult.Message,
                errorDetails: stampResult.ErrorDetails!,
                statusCode: stampResult.StatusCode);

        AuditLog auditLog = new(description, entityId, model.TenantId, stampResult.Data!);

        Result validationResult = Validate(auditLog);

        if (validationResult.IsFailure)
            return Result<AuditLog>.Failure(
                message: validationResult.Message,
                errorDetails: validationResult.ErrorDetails!,
                statusCode: validationResult.StatusCode);

        auditLog.Raise(new AuditLogCreatedDomainEvent(auditLog.Id));

        return Result<AuditLog>.Success(
            message: "Audit log created successfully",
            data: auditLog);
    }   
}

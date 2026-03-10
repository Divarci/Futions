using Core.Domain.Entities.System.AuditLogs.DomainEvents;
using Core.Domain.Entities.System.AuditLogs.Models;
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

    private AuditLog(Guid tenantId, AuditStamp created)
    {
        TenantId = tenantId;
        Created = created;
    }

    // Properties
    public AuditStamp Created { get; private set; } = default!;
    public AuditStamp? Updated { get; private set; }

    // IHaveTenant properties
    public Guid TenantId { get; private set; }

    // Methods
    public static Result<AuditLog> Create(AuditLogCreateModel model)
    {
        if (model is null)
            return Result<AuditLog>.Failure(
                message: "Model can not be null",
                statusCode: HttpStatusCode.InternalServerError);

        Result<AuditStamp> stampResult = AuditStamp.Create(model.CreatedStampModel);

        if (stampResult.IsFailure)
            return Result<AuditLog>.Failure(
                message: stampResult.Message,
                errorDetails: stampResult.ErrorDetails!,
                statusCode: stampResult.StatusCode);

        AuditLog auditLog = new( model.TenantId, stampResult.Data!);

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

    public Result SetUpdated(AuditStampModel stampModel)
    {
        if (stampModel is null)
            return Result<AuditLog>.Failure(
                message: "Model can not be null",
                statusCode: HttpStatusCode.InternalServerError);

        Result<AuditStamp> stampResult = AuditStamp.Create(stampModel);

        if (stampResult.IsFailure)
            return Result.Failure(
                message: stampResult.Message,
                errorDetails: stampResult.ErrorDetails!,
                statusCode: stampResult.StatusCode);

        Updated = stampResult.Data!;

        Raise(new AuditLogUpdatedDomainEvent(Id));

        return Result.Success("Audit log updated successfully");
    }
}

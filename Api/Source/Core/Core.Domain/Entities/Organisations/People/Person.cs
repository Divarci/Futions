using Core.Domain.Entities.Organisations.People.DomainEvents;
using Core.Domain.Entities.Organisations.People.Models;
using Core.Domain.ValueObjects.FullnameValueObject;
using Core.Library.Abstractions;
using Core.Library.Abstractions.Interfaces;
using Core.Library.ResultPattern;
using System.Net;

namespace Core.Domain.Entities.Organisations.People;

public partial class Person : BaseEntity, IHaveSoftDelete, IHaveTenant
{
    // Constructors
    private Person() { }

    private Person(Guid tenantId, Fullname fullname)
    {
        TenantId = tenantId;
        Fullname = fullname;
    }

    // Properties
    public Fullname Fullname { get; private set; } = default!;
    public string? Email { get; private set; }

    // IHaveSoftDelete Properties
    public bool IsDeleted { get; private set; }

    // IHaveTenant Properties
    public Guid TenantId { get; private set; }

    // Methods
    public static Result<Person> Create(PersonCreateModel model)
    {
        Result<Fullname> fullnameResult = Fullname.Create(model.FullnameModel);

        if (fullnameResult.IsFailure)
            return Result<Person>.Failure(
                message: fullnameResult.Message,
                errorDetails: fullnameResult.ErrorDetails!,
                statusCode: fullnameResult.StatusCode);

        Person person = new(model.TenantId, fullnameResult.Data!);

        Result validationResult = Validate(person);

        if (validationResult.IsFailure)
            return Result<Person>.Failure(
                message: validationResult.Message,
                errorDetails: validationResult.ErrorDetails!,
                statusCode: validationResult.StatusCode);

        person.Raise(new PersonCreatedDomainEvent(person.Id));

        return Result<Person>.Success(
            message: "Person created successfully",
            data: person,
            statusCode: HttpStatusCode.OK);
    }

    public Result SoftDelete()
    {
        if (IsDeleted)
            return Result.Failure(
                message: "Person is already deleted",
                statusCode: HttpStatusCode.BadRequest);

        IsDeleted = true;

        Raise(new PersonDeletedDomainEvent(Id));

        return Result.Success(
            message: "Person deleted successfully",
            statusCode: HttpStatusCode.OK);
    }

    public Result UpdateFullname(FullnameModel fullnameModel)
    {
        if (IsDeleted)
            return Result.Failure(
                message: "Cannot update a deleted person",
                statusCode: HttpStatusCode.BadRequest);

        Result<Fullname> fullnameResult = Fullname.Create(fullnameModel);

        if (fullnameResult.IsFailure)
            return fullnameResult;

        Result validationResult = Validate(this);

        if (validationResult.IsFailure)
            return validationResult;

        Fullname = fullnameResult.Data!;

        Raise(new PersonFullnameUpdatedDomainEvent(Id));

        return Result.Success(
            message: "Person fullname updated successfully",
            statusCode: HttpStatusCode.OK);
    }

    public Result UpdateEmail(string email)
    {
        if (IsDeleted)
            return Result.Failure(
                message: "Cannot update a deleted person",
                statusCode: HttpStatusCode.BadRequest);

        Email = email;

        Result validationResult = Validate(this);

        if (validationResult.IsFailure)
            return validationResult;

        Raise(new PersonEmailUpdatedDomainEvent(Id));

        return Result.Success(
            message: "Person email updated successfully",
            statusCode: HttpStatusCode.OK);
    }
}

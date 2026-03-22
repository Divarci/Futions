using Core.Library.Exceptions;
using Core.Library.ResultPattern;
using System.Net;

namespace Core.Domain.ValueObjects.FullnameValueObject;

public sealed partial record Fullname
{
    // Constructor
    private Fullname(string firstName, string lastName)
    {
        FirstName = firstName;
        LastName = lastName;
    }

    // Properties
    public string FirstName { get; init; }
    public string LastName { get; init; }

    // Methods
    public static Result<Fullname> Create(FullnameModel model)
    {
        if (model is null)
            throw new FutionsException(
                assemblyName: "Core.Domain",
                className: nameof(Fullname),
                methodName: nameof(Create),
                message: "Create model cannot be null.");

        Fullname fullname = new(
            model.FirstName!,
            model.LastName!);

        Result validationResult = Validate(fullname);

        if (validationResult.IsFailure)
            return Result<Fullname>.Failure(
                message: validationResult.Message,
                errorDetails: validationResult.ErrorDetails!,
                statusCode: validationResult.StatusCode);

        return Result<Fullname>.Success(
            message: "Fullname created successfully",
            data: fullname,
            statusCode: HttpStatusCode.OK);
    }   
}

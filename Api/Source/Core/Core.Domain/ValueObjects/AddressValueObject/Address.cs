using Core.Library.Exceptions;
using Core.Library.ResultPattern;
using System.Net;

namespace Core.Domain.ValueObjects.AddressValueObject;

public sealed partial record Address
{
    // Constructor
    private Address(string lineOne, string? lineTwo,
        string? lineThree, string? lineFour, string postcode)
    {
        LineOne = lineOne;
        LineTwo = lineTwo;
        LineThree = lineThree;
        LineFour = lineFour;
        Postcode = postcode;
    }

    // Properties
    public string LineOne { get; init; }
    public string? LineTwo { get; init; }
    public string? LineThree { get; init; }
    public string? LineFour { get; init; }
    public string Postcode { get; init; }

    // Methods
    public static Result<Address> Create(AddressModel model)
    {
        if (model is null)
            throw new FutionsException(
                assemblyName: "Core.Domain",
                className: nameof(Address),
                methodName: nameof(Create),
                message: "Create model cannot be null.");

        Address adress = new(
            model.LineOne!,
            model.LineTwo,
            model.LineThree,
            model.LineFour,
            model.Postcode!);

        Result validationResult = Validate(adress);

        if (validationResult.IsFailure)
            return Result<Address>.Failure(
                message: validationResult.Message,
                errorDetails: validationResult.ErrorDetails!,
                statusCode: validationResult.StatusCode);

        return Result<Address>.Success(
            message: "Address created successfully",
            data: adress,
            statusCode: HttpStatusCode.OK);
    }
}
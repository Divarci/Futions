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
    public string LineOne { get; private set; }
    public string? LineTwo { get; private set; }
    public string? LineThree { get; private set; }
    public string? LineFour { get; private set; }
    public string Postcode { get; private set; }

    // Methods
    public static Result<Address> Create(AddressModel model)
    {
        if (model is null)
            return Result<Address>.Failure(
                message: "Model can not be null",
                statusCode: HttpStatusCode.InternalServerError);

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
            data: adress);
    }  
}
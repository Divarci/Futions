using Core.Library.ResultPattern;
using Core.Library.Validators;
using System.Net;

namespace Core.Domain.Entities.Organisations.Products;

public partial class Product
{
    private static Result Validate(Product product)
    {
        List<Result> results = [];

        ValidateProperties(results, product);
        ValidateBusiness(results, product);

        return Result.CombineValidationErrors(results);
    }

    private static void ValidateProperties(List<Result> results, Product product)
        => results.AddRange(
            [
                product.TenantId.Validate(
                    fieldName: nameof(product.TenantId),
                    allowEmpty: false),
                product.Name.Validate(
                    fieldName: nameof(product.Name),
                    maxLength: 100),
                product.Price.Validate(
                    fieldName: nameof(product.Price)),
            ]);

    private static void ValidateBusiness(List<Result> results, Product product)
    {
        if (product.Name.StartsWith("X"))
            results.Add(Result.Failure(
                message: "Validation failed",
                errorDetails: ErrorDetails.Create(["Product name cannot start with X."]),
                statusCode: HttpStatusCode.UnprocessableContent));
    }
}

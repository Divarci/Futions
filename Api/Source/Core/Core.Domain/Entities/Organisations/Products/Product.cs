using Core.Domain.Entities.Organisations.Companies;
using Core.Domain.Entities.Organisations.Products.DomainEvents;
using Core.Domain.Entities.Organisations.Products.Models;
using Core.Library.Abstractions;
using Core.Library.Abstractions.Interfaces;
using Core.Library.ResultPattern;
using System.Net;
using System.Text.Json.Serialization;

namespace Core.Domain.Entities.Organisations.Products;

public partial class Product : BaseEntity, IHaveSoftDelete, IHaveTenant
{
    // Constructors
    private Product() { }

    [JsonConstructor]
    private Product(Guid tenantId, string name, decimal price, Guid companyId)
    {
        TenantId = tenantId;
        Name = name;
        Price = price;
        CompanyId = companyId;
    }

    // Properties
    public Guid CompanyId { get; private set; }
    public string Name { get; private set; } = default!;
    public decimal Price { get; private set; }

    // Navigation Properties
    public Company Company { get; private set; } = default!;

    // IHaveSoftDelete Properties
    public bool IsDeleted { get; private set; }

    // IHaveTenant Properties
    public Guid TenantId { get; private set; }

    // Methods
    public static Result<Product> Create(ProductCreateModel model)
    {
        Product product = new(model.TenantId, model.Name, model.Price, model.CompanyId);

        Result validationResult = Validate(product);

        if (validationResult.IsFailure)
            return Result<Product>.Failure(
                message: validationResult.Message,
                errorDetails: validationResult.ErrorDetails!,
                statusCode: validationResult.StatusCode);

        product.Raise(new ProductCreatedDomainEvent(product.Id));

        return Result<Product>.Success(
            message: "Product created successfully",
            data: product,
            statusCode: HttpStatusCode.OK);
    }

    public Result SoftDelete()
    {
        if (IsDeleted)
            return Result.Failure(
                message: "Product is already deleted",
                statusCode: HttpStatusCode.BadRequest);

        IsDeleted = true;

        Raise(new ProductDeletedDomainEvent(Id));

        return Result.Success(
            message: "Product deleted successfully",
            statusCode: HttpStatusCode.OK);
    }

    public Result UpdateName(string name)
    {
        if (IsDeleted)
            return Result.Failure(
                message: "Cannot update a deleted product",
                statusCode: HttpStatusCode.BadRequest);

        Name = name;

        Result validationResult = Validate(this);

        if (validationResult.IsFailure)
            return validationResult;

        Raise(new ProductNameUpdatedDomainEvent(Id));

        return Result.Success(
            message: "Product name updated successfully",
            statusCode: HttpStatusCode.OK);
    }

    public Result UpdatePrice(decimal price)
    {
        if (IsDeleted)
            return Result.Failure(
                message: "Cannot update a deleted product",
                statusCode: HttpStatusCode.BadRequest);

        Price = price;

        Result validationResult = Validate(this);

        if (validationResult.IsFailure)
            return validationResult;

        Raise(new ProductPriceUpdatedDomainEvent(Id));

        return Result.Success(
            message: "Product price updated successfully",
            statusCode: HttpStatusCode.OK);
    }
}

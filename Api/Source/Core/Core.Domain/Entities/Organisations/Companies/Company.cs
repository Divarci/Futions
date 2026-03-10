using Core.Domain.Entities.Organisations.Companies.DomainEvents;
using Core.Domain.Entities.Organisations.Companies.Models;
using Core.Domain.Entities.Organisations.Products;
using Core.Domain.ValueObjects.AddressValueObject;
using Core.Library.Abstractions;
using Core.Library.Abstractions.Interfaces;
using Core.Library.ResultPattern;
using System.Net;

namespace Core.Domain.Entities.Organisations.Companies;

public partial class Company : BaseEntity, IHaveSoftDelete, IHaveTenant
{
    // Constructors
    private Company() { }

    private Company(Guid tenantId, string name, Address address)
    {
        Name = name;
        Address = address;
        TenantId = tenantId;
    }

    // Properties
    public string Name { get; private set; } = default!;
    public Address Address { get; private set; } = default!;

    // Navigation Properties
    public ICollection<Product> Products { get; private set; } = [];

    // IHaveSoftDelete Properties
    public bool IsDeleted { get; private set; }

    // IHaveTenant Properties
    public Guid TenantId { get; private set; } = default!;

    // Methods
    public static Result<Company> Create(CompanyCreateModel model)
    {
        Result<Address> addressResult = Address.Create(model.AddressModel);

        if (addressResult.IsFailure)
            return Result<Company>.Failure(
                message: addressResult.Message,
                errorDetails: addressResult.ErrorDetails!,
                statusCode: addressResult.StatusCode);

        Company company = new(model.TenantId, model.Name, addressResult.Data!);

        Result validationResult = Validate(company);

        if (validationResult.IsFailure)
            return Result<Company>.Failure(
                message: validationResult.Message,
                errorDetails: validationResult.ErrorDetails!,
                statusCode: validationResult.StatusCode);

        company.Raise(new CompanyCreatedDomainEvent(company.Id));

        return Result<Company>.Success(
            message: "Company created successfully",
            data: company);
    }

    public Result SoftDelete()
    {
        if (IsDeleted)
            return Result.Failure(
                message: "Company is already deleted",
                statusCode: HttpStatusCode.BadRequest);

        IsDeleted = true;

        Raise(new CompanyDeletedDomainEvent(Id));

        return Result.Success("Company deleted successfully");
    }

    public Result UpdateName(string name)
    {
        if (IsDeleted)
            return Result.Failure(
                message: "Cannot update a deleted company",
                statusCode: HttpStatusCode.BadRequest);

        Name = name;

        Result validationResult = Validate(this);

        if (validationResult.IsFailure)
            return validationResult;
        
        Raise(new CompanyNameUpdatedDomainEvent(Id));

        return Result.Success("Company name updated successfully");
    }

    public Result UpdateAddress(AddressModel addressModel)
    {
        if (IsDeleted)
            return Result.Failure(
                message: "Cannot update a deleted company",
                statusCode: HttpStatusCode.BadRequest);

        Result<Address> addressResult = Address.Create(addressModel);

        if (addressResult.IsFailure)
            return addressResult;

        Result validationResult = Validate(this);

        if (validationResult.IsFailure)
            return validationResult;

        Address = addressResult.Data!;

        Raise(new CompanyAddressUpdatedDomainEvent(Id));

        return Result.Success("Company address updated successfully");
    }
}
using Core.Domain.Entities.Organisations.Companies;
using Core.Domain.Entities.Organisations.CompanyPeople.DomainEvents;
using Core.Domain.Entities.Organisations.CompanyPeople.Models;
using Core.Domain.Entities.Organisations.People;
using Core.Library.Abstractions;
using Core.Library.ResultPattern;
using System.Net;

namespace Core.Domain.Entities.Organisations.CompanyPeople;

public sealed partial class CompanyPerson : BaseEntity
{
    // Constructors
    private CompanyPerson() { }

    private CompanyPerson(Guid companyId, Guid personId, string title)
    {        
        CompanyId = companyId;
        PersonId = personId;
        Title = title;
    }

    // Properties
    public Guid CompanyId { get; private set; }
    public Guid PersonId { get; private set; }
    public string Title { get; private set; } = default!;

    // Navigation Properties
    public Company Company { get; private set; } = default!;
    public Person Person { get; private set; } = default!;
      
    // Methods
    public static Result<CompanyPerson> Create(CompanyPersonCreateModel model)
    {
        CompanyPerson companyPerson = new(model.CompanyId, model.PersonId, model.Title);

        Result validationResult = Validate(companyPerson);

        if (validationResult.IsFailure)
            return Result<CompanyPerson>.Failure(
                message: validationResult.Message,
                errorDetails: validationResult.ErrorDetails!,
                statusCode: validationResult.StatusCode);

        companyPerson.Raise(new CompanyPersonCreatedDomainEvent(companyPerson.Id));

        return Result<CompanyPerson>.Success(
            message: "Company person created successfully",
            data: companyPerson,
            statusCode: HttpStatusCode.OK);
    }

    public Result Delete()
    {
        Raise(new CompanyPersonDeletedDomainEvent(Id));

        return Result.Success(
            message: "Company person deleted successfully",
            statusCode: HttpStatusCode.OK);
    }

    public Result UpdateTitle(string title)
    {
        Title = title;

        Result validationResult = Validate(this);

        if (validationResult.IsFailure)
            return validationResult;

        Raise(new CompanyPersonTitleUpdatedDomainEvent(Id));

        return Result.Success(
            message: "Company person title updated successfully",
            statusCode: HttpStatusCode.OK);
    }
}

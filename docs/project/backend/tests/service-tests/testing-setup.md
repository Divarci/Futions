## Testing Setup

**Base Class - Mock Setup and Helpers**

The base class sets up all mocks and provides reusable helper methods.

**CRITICAL RULE:** Followings are base guidelines for each service. If a domain has unique business rules, domain **MUST BE** deeply investigated for accurate helper methods. 

**Example 1:**
```csharp
public static Company CreateActiveCompanyWithDefaultUser(
    string name = "Test Company",
    Guid? id = null)    
{
    var company = CreateValidCompany(name, id);

    company.Activate();
    company.AddUser();

    return company;
}
```

**Example 2:**
```csharp
using FluentAssertions;
using Futions.Crm.Core.App.Companies.Services;
using Futions.Crm.Core.Domain.Abstractions.Interfaces.UnitOfWork;
using Futions.Crm.Core.Domain.Companies;
using Futions.Crm.Core.Domain.Entities.Organisations.Companies;
using Futions.Crm.Core.Domain.Entities.Organisations.Companies.Interfaces.Repositories;
using Futions.Crm.Core.Domain.Entities.Organisations.Companies.Models;
using Futions.Crm.Core.Domain.Entities.Organisations.People.Interfaces.Repositories;
using Futions.Crm.Core.Domain.Entities.Organisations.People.Models;
using Futions.Crm.Core.Domain.People;
using Futions.Crm.Core.Domain.ResultPattern;
using Futions.Crm.Core.Domain.ValueObjects.AddressValueObject;
using Futions.Crm.Core.Domain.ValueObjects.FullnameValueObject;
using Moq;

namespace Futions.Crm.App.Tests.Services.Organisations.Companies;

/// <summary>
/// Tests for CompanyService
/// Base class contains mock setup and helper methods
/// Repository interfaces are from: Entities/{Entity}/Interfaces/Repositories/
/// </summary>
public partial class CompanyServiceTests
{
    #region Mock Setup

    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<ICompanyRepository> _repositoryMock;
    private readonly Mock<IPersonRepository> _personRepositoryMock;
    private readonly Mock<ICompanyPersonRepository> _companyPersonRepositoryMock;
    private readonly CompanyService _sut; // System Under Test

    public CompanyServiceTests()
    {
        // Setup mocks - use entity-specific repository interfaces when available
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _repositoryMock = new Mock<ICompanyRepository>();
        _personRepositoryMock = new Mock<IPersonRepository>();
        _companyPersonRepositoryMock = new Mock<ICompanyPersonRepository>();

        // Create system under test
        _sut = new CompanyService(
            _repositoryMock.Object,
            _personRepositoryMock.Object,
            _companyPersonRepositoryMock.Object,
            _unitOfWorkMock.Object);
    }

    #endregion

    #region Helper Methods - Models

    /// <summary>
    /// Creates a valid CreateCompanyModel for testing
    /// </summary>
    private static CreateCompanyModel CreateValidCompanyModel(string name = "Test Company")
    {
        return new CreateCompanyModel { Name = name };
    }

    /// <summary>
    /// Creates a valid UpdateCompanyModel for testing
    /// </summary>
    private static UpdateCompanyModel CreateValidUpdateCompanyModel(Guid companyId, string? name = null, AddressModel? addressModel = null)
    {
        return new UpdateCompanyModel
        {
            CompanyId = companyId,
            Name = name,
            AddressModel = addressModel
        };
    }

    /// <summary>
    /// Creates a valid UpdateCompanyPersonModel for testing
    /// </summary>
    private static UpdateCompanyPersonModel CreateValidUpdateCompanyPersonModel(Guid companyPersonId, string? role = null)
    {
        return new UpdateCompanyPersonModel
        {
            CompanyPersonId = companyPersonId,
            Role = role
        };
    }

    /// <summary>
    /// Creates a valid AddressModel for testing
    /// </summary>
    private static AddressModel CreateValidAddressModel()
    {
        return new AddressModel
        {
            LineOne = "123 Main St",
            LineTwo = "Suite 100",
            LineThree = null,
            LineFour = null,
            Postcode = "12345"
        };
    }

    #endregion

    #region Helper Methods - Entities

    /// <summary>
    /// Creates a valid Company entity for testing
    /// </summary>
    private static Company CreateValidCompany(Guid? tenantId = null, string name = "Test Company", Guid? id = null)
    {
        var model = CreateValidCompanyModel(name);
        var result = Company.Create(model, tenantId ?? Guid.NewGuid());
        var company = result.Data!;

        // Set ID if provided using reflection
        if (id.HasValue)
        {
            var idProperty = typeof(Company).GetProperty("Id");
            idProperty?.SetValue(company, id.Value);
        }

        return company;
    }

    /// <summary>
    /// Creates a valid Person entity for testing
    /// </summary>
    private static Person CreateValidPerson(Guid? tenantId = null, string firstName = "John", string lastName = "Doe", Guid? id = null)
    {
        var fullnameModel = new FullnameModel { Firstname = firstName, Lastname = lastName };
        var model = new CreatePersonModel { FullnameModel = fullnameModel };
        var person = Person.Create(model, tenantId ?? Guid.NewGuid()).Data!;

        // Set ID if provided using reflection
        if (id.HasValue)
        {
            var idProperty = typeof(Person).GetProperty("Id");
            idProperty?.SetValue(person, id.Value);
        }

        return person;
    }

    /// <summary>
    /// Creates a valid CompanyPerson relationship for testing
    /// </summary>
    private static CompanyPerson CreateValidCompanyPerson(Guid tenantId, Guid companyId, Guid personId, Guid? id = null)
    {
        var companyPerson = CompanyPerson.Create(companyId, personId, tenantId).Data!;

        // Set ID if provided using reflection
        if (id.HasValue)
        {
            var idProperty = typeof(CompanyPerson).GetProperty("Id");
            idProperty?.SetValue(companyPerson, id.Value);
        }

        return companyPerson;
    }

    #endregion

    #region Helper Methods - IDs

    /// <summary>
    /// Gets a valid tenant ID for testing
    /// </summary>
    private static Guid GetValidTenantId()
    {
        return Guid.NewGuid();
    }

    /// <summary>
    /// Gets a valid company ID for testing
    /// </summary>
    private static Guid GetValidCompanyId()
    {
        return Guid.NewGuid();
    }

    /// <summary>
    /// Gets a valid person ID for testing
    /// </summary>
    private static Guid GetValidPersonId()
    {
        return Guid.NewGuid();
    }

    /// <summary>
    /// Gets a valid company person ID for testing
    /// </summary>
    private static Guid GetValidCompanyPersonId()
    {
        return Guid.NewGuid();
    }

    #endregion

    #region Helper Methods - Assertions

    /// <summary>
    /// Asserts that a result is successful
    /// </summary>
    private static void AssertSuccessResult(Result result)
    {
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.IsFailed.Should().BeFalse();
        result.Error.Should().BeNull();
    }

    /// <summary>
    /// Asserts that a result is successful with data
    /// </summary>
    private static void AssertSuccessResult<T>(Result<T> result)
    {
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.IsFailed.Should().BeFalse();
        result.Error.Should().BeNull();
        result.Data.Should().NotBeNull();
    }   

    /// <summary>
    /// Asserts that a result has failed with expected error type
    /// </summary>
    private static void AssertFailureResult(Result result, ErrorType expectedErrorType)
    {
        result.Should().NotBeNull();
        result.IsFailed.Should().BeTrue();
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().NotBeNull();
        result.Error!.Code.Should().Be(expectedErrorType);
    }

    /// <summary>
    /// Asserts that a result has failed with expected error type
    /// </summary>
    private static void AssertFailureResult<T>(Result<T> result, ErrorType expectedErrorType)
    {
        result.Should().NotBeNull();
        result.IsFailed.Should().BeTrue();
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().NotBeNull();
        result.Error!.Code.Should().Be(expectedErrorType);
        result.Data.Should().BeNull();
    }

    #endregion
}
```

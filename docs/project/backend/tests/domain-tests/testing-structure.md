## Project Structure

Domain tests follow **vertical slice architecture** with **operation-based organization** and **partial classes**:

### Complete Example: Tenant Entity

This example shows the complete structure for the **Tenant** entity, demonstrating how domain entities with partial classes are organized:

#### Domain Entity Files (Production Code)
```
api/src/Core/Futions.Crm.Core.Domain/
└── Entities/
    └── Identity/
        └── Tenants/
            ├── Tenant.cs                           # Main entity file
            ├── Tenant.Subscription.cs              # Partial class for subscription operations
            ├── Tenant.User.cs                      # Partial class for user management operations
            ├── DomainEvents/
            │   └── TenantCreatedDomainEvent.cs     # Domain event raised on creation
            ├── Interfaces/
            │   ├── Repositories/
            │   │   └── ITenantRepository.cs        # Repository interface
            │   └── Services/
            │       └── ITenantService.cs           # Service interface
            └── Models/
                ├── CreateTenantModel.cs            # DTO for creation
                └── UpdateTenantModel.cs            # DTO for updates
```

#### Test Files Structure (Test Code)
```
api/test/Core/Futions.Crm.Domain.Tests/
└── Entities/
    └── Identity/
        └── Tenant/
            ├── TenantTests.cs                                      # Base class + helpers + setup
            ├── Create/                                             # From Tenant.cs
            │   ├── TenantTests.Create.Success.cs
            │   └── TenantTests.Create.Failure.cs
            ├── Update/                                             # From Tenant.cs
            │   ├── UpdateName/
            │   │   ├── TenantTests.UpdateName.Success.cs
            │   │   └── TenantTests.UpdateName.Failure.cs
            │   └── UpdateStatus/
            │       ├── TenantTests.UpdateStatus.Success.cs
            │       └── TenantTests.UpdateStatus.Failure.cs
            ├── Activate/                                           # From Tenant.cs
            │   ├── TenantTests.Activate.Success.cs
            │   └── TenantTests.Activate.Failure.cs
            ├── Deactivate/                                         # From Tenant.cs
            │   ├── TenantTests.Deactivate.Success.cs
            │   └── TenantTests.Deactivate.Failure.cs
            ├── Delete/                                             # From Tenant.cs
            │   ├── TenantTests.Delete.Success.cs
            │   └── TenantTests.Delete.Failure.cs
            ├── Subscription/                                       # From Tenant.Subscription.cs (partial)
            │   ├── AddSubscription/
            │   │   ├── TenantTests.AddSubscription.Success.cs
            │   │   └── TenantTests.AddSubscription.Failure.cs
            │   ├── RemoveSubscription/
            │   │   ├── TenantTests.RemoveSubscription.Success.cs
            │   │   └── TenantTests.RemoveSubscription.Failure.cs
            │   └── UpdateSubscription/
            │       ├── TenantTests.UpdateSubscription.Success.cs
            │       └── TenantTests.UpdateSubscription.Failure.cs
            └── User/                                               # From Tenant.User.cs (partial)
                ├── AddUser/
                │   ├── TenantTests.AddUser.Success.cs
                │   └── TenantTests.AddUser.Failure.cs
                ├── RemoveUser/
                │   ├── TenantTests.RemoveUser.Success.cs
                │   └── TenantTests.RemoveUser.Failure.cs
                └── IncreaseUserLimit/
                    ├── TenantTests.IncreaseUserLimit.Success.cs
                    └── TenantTests.IncreaseUserLimit.Failure.cs
```

### Complete Example: Fullname Value Object

This example shows the complete structure for the **Fullname** value object:

#### Domain Value Object Files (Production Code)
```
api/src/Core/Futions.Crm.Core.Domain/
└── ValueObjects/
    └── FullnameValueObject/
        ├── Fullname.cs                             # Value object implementation
        └── FullnameModel.cs                        # DTO/Model for creation
```

#### Test Files Structure (Test Code)
```
api/test/Core/Futions.Crm.Domain.Tests/
└── ValueObjects/
    └── Fullname/
        ├── FullnameTests.cs                        # Base class + helpers + setup
        ├── Create/
        │   ├── FullnameTests.Create.Success.cs     # Valid creation scenarios
        │   └── FullnameTests.Create.Failure.cs     # Validation failures
        └── Update/
            ├── FullnameTests.Update.Success.cs     # Valid update scenarios
            └── FullnameTests.Update.Failure.cs     # Update validation failures
```

### Complete Example: StringValidator

This example shows the complete structure for validators:

```
api/test/Core/Futions.Crm.Domain.Tests/
└── Validators/
    └── StringValidator/
        ├── StringValidatorTests.cs                           # Base class + helpers
        └── Validate/
            ├── StringValidatorTests.Validate.Success.cs      # All success scenarios
            └── StringValidatorTests.Validate.Failure.cs      # All failure scenarios
```

### When to Use Business Domain Folders

**Business domain folders** are used when an entity has **partial classes** that group methods by business concern:

#### Use Domain Folders When:
1. ✅ Entity has partial classes (e.g., `Company.Person.cs`, `Tenant.Subscription.cs`, `Tenant.User.cs`)
2. ✅ Methods are cohesively related to a specific business domain
3. ✅ Grouping improves navigation and logical organization

#### Example Mapping - Partial Class to Test Folder:
```
Entity Partial Classes          →  Test Folder Structure
─────────────────────────────────────────────────────────
Company.cs (main)               →  Company/Create/, Company/Update/
Company.Person.cs (partial)     →  Company/Person/AddPerson/, Company/Person/RemovePerson/

Tenant.cs (main)                →  Tenant/Create/, Tenant/Activate/
Tenant.Subscription.cs          →  Tenant/Subscription/AddSubscription/
Tenant.User.cs                  →  Tenant/User/AddUser/, Tenant/User/RemoveUser/
```

### Join Table Entities

**Join table entities** (e.g., `CompanyPerson`, `UserRole`) are **standalone entities** and should have their own **entity-level folders**, even if they're physically located in another entity's folder.

#### Join Table Rules:
1. ✅ Create a separate entity-level folder (e.g., `CompanyPerson/`, not nested under `Company/`)
2. ✅ Follow the same test structure as other entities (Create, Update, Delete, etc.)
3. ✅ Test all public methods including join-specific operations (e.g., `Restore()` for soft-deleted joins)
4. ✅ Use a unique namespace to avoid conflicts (e.g., `CompanyPersonEntity` instead of `CompanyPerson`)

#### Example - Join Table Entity Structure:
```
Domain Entity Location          →  Test Folder Structure
─────────────────────────────────────────────────────────
Entities/Organisations/
  Companies/CompanyPerson.cs    →  Entities/Organisations/CompanyPerson/
                                   ├── CompanyPersonTests.cs
                                   ├── Create/
                                   ├── Update/UpdateRole/
                                   ├── Delete/
                                   └── Restore/

Identity/Users/UserRole.cs      →  Entities/Identity/UserRole/
                                   ├── UserRoleTests.cs
                                   └── Create/
```

**Note**: Join tables can have different complexity levels:
- **Complex**: CompanyPerson has Create, Update, Delete, and Restore operations
- **Simple**: UserRole only has Create operation (no soft delete or updates)

Test structure adapts to the entity's actual public methods.

**Why Separate Folders for Join Tables?**
- **Clarity**: Join tables are entities with their own lifecycle and business rules
- **Discoverability**: Easier to find and navigate join table tests
- **Consistency**: Follows the same pattern as other entities
- **Scalability**: Join tables can grow to have complex logic independent of parent entities

### Structure Rules

**CRITICAL:** Single structure - use only the example above.

1. **Base class** : Cotains helper methods
2. **No exceptions** : Do not skip folders; every method must have Success and Failure files.
3. **Method-Level Folders** : Each method gets its own folder
4. **Tests must be pure** : Do not use mocks in domain entity/value object/validator tests.

**Why Only Success.cs and Failure.cs?**:
- **Consistency**: Every operation follows the same pattern
- **Clarity**: Immediately clear where to find tests (success vs failure scenarios)
- **Maintenance**: Easier to locate and update tests
- **Review**: Code reviews are simpler with predictable structure
- **No Confusion**: Additional.cs, PropertyVerification.cs, etc. create ambiguity about where tests belong

**If You Need More Organization**:
- Use #region comments within Success.cs or Failure.cs
- Group related tests together with clear comments
- Do NOT create additional partial files - it violates the structure standard

**Remember:** Consistency is key. Follow this structure for all domain tests to maintain a predictable and maintainable test suite.

---

## Domain Architecture Rules

### Why Partial Classes?

**Partial classes** are used in the domain layer to organize complex entities by **business concerns**:

1. **Separation of Concerns**: Each partial class file groups related operations
   - `Tenant.cs` - Core entity operations (Create, Update, Delete, Activate)
   - `Tenant.Subscription.cs` - Subscription management operations
   - `Tenant.User.cs` - User management operations

2. **Maintainability**: Easier to navigate and maintain large entities with many methods

3. **Team Collaboration**: Multiple developers can work on different aspects of the same entity without merge conflicts

4. **Logical Grouping**: Methods are grouped by business domain, not technical concerns

**Example**:
```csharp
// Tenant.cs - Main entity
public sealed partial class Tenant : BaseEntity
{
    public static Result<Tenant> Create(...) { }
    public Result UpdateName(string name) { }
    public Result Activate() { }
}

// Tenant.Subscription.cs - Subscription operations
public sealed partial class Tenant
{
    public Result AddSubscription(...) { }
    public Result RemoveSubscription(...) { }
}

// Tenant.User.cs - User operations  
public sealed partial class Tenant
{
    public Result AddUser(...) { }
    public Result RemoveUser(...) { }
}
```

### Vertical Slice Architecture

**Vertical Slice** means organizing code by **feature/business capability** rather than technical layers:

#### Traditional Layered Approach (❌):
```
Entities/
├── Company.cs
├── Person.cs
├── Deal.cs
└── Tenant.cs
```

#### Vertical Slice Approach (✅):
```
Entities/
├── Organisations/              # Business domain
│   ├── Companies/
│   │   ├── Company.cs
│   │   └── CompanyPerson.cs
│   └── People/
│       └── Person.cs
├── Identity/                   # Business domain
│   └── Tenants/
│       ├── Tenant.cs
│       ├── Tenant.Subscription.cs
│       └── Tenant.User.cs
└── Crm/                        # Business domain
    └── Deals/
        ├── Deal.cs
        └── DealProduct.cs
```

**Benefits**:
- **Discoverability**: Related entities are co-located
- **Business Alignment**: Structure mirrors business domains
- **Scalability**: Easy to add new features within their domain
- **Team Organization**: Teams can own specific domains

### Naming Conventions

#### Test Class Names
- **Format**: `{EntityName}Tests.cs`
- **Examples**: `TenantTests.cs`, `FullnameTests.cs`, `StringValidatorTests.cs`

#### Test Method Names
- **Format**: `{MethodUnderTest}_{Scenario}_{ExpectedResult}`
- **Examples**:
  - `Create_WithValidData_ReturnsSuccessResult`
  - `AddUser_WhenLimitReached_ReturnsConflictError`
  - `Validate_WhenRequiredAndNull_ReturnsValidationError`

#### File Organization
- **Base class**: Contains helper methods and setup
- **Success.cs**: All success scenario tests
- **Failure.cs**: All failure scenario tests

### Code Style and Patterns

#### AAA Pattern (Arrange-Act-Assert)
All tests follow the AAA pattern for clarity:

```csharp
[Fact]
public void Create_WithValidData_ReturnsSuccessResult()
{
    // Arrange - Setup test data
    CreateTenantModel model = CreateValidTenantModel();
    
    // Act - Execute the operation
    Result<Tenant> result = Tenant.Create(model);
    
    // Assert - Verify the outcome
    result.IsSuccess.Should().BeTrue();
    result.Data.Should().NotBeNull();
    result.Data!.Name.Should().Be(model.Name);
}
```

#### Result Pattern
All domain operations return `Result` or `Result<T>`:
- **Success**: `Result.Success()` or `Result<T>.Success(data)`
- **Failure**: `Result.Failure(error)` or `Result<T>.Failure(error)`

#### No Mocking in Domain Tests
Domain tests are **pure unit tests** - they test business logic without external dependencies:
- ✅ Test entity methods directly
- ✅ Test value object creation and validation
- ✅ Test validators
- ❌ No repository mocks
- ❌ No service mocks
- ❌ No database access

#### Helper Methods
Base test classes contain helper methods to:
- Create valid test data
- Generate test entities
- Create strings of specific lengths
- Reduce duplication across tests

**Example**:
```csharp
public partial class TenantTests
{
    protected static CreateTenantModel CreateValidTenantModel(
        string name = "Test Tenant",
        int userLimit = 10)
    {
        return new CreateTenantModel
        {
            Name = name,
            UserLimit = userLimit
        };
    }
    
    protected static string CreateStringOfLength(int length)
    {
        return new string('A', length);
    }
}
```

### Structure Consistency

**CRITICAL**: All domain tests follow the same structure:

1. **One base class** per entity/value object/validator
2. **Method-level folders** for each operation
3. **Success.cs and Failure.cs** split for each method
4. **No additional partial files** - use regions within Success/Failure files if needed
5. **Test files mirror domain structure** - same module organization

This consistency ensures:
- Predictable navigation
- Easier code reviews
- Simplified maintenance
- Clear expectations for all developers

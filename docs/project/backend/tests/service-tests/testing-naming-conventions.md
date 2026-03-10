## Test Naming Conventions

**Pattern**: `{MethodUnderTest}_{Scenario}_{ExpectedResult}`

**Examples:**
```csharp
// Create
CreateCompanyForTenantAsync_WithValidData_ReturnsSuccessResult()
CreateCompanyForTenantAsync_WithAllInvalidInputs_ReturnsValidationErrorWithCorrectCount()

// Single
GetCompanyByIdForTenantAsync_WithValidId_ReturnsSuccessResult()
GetCompanyByIdForTenantAsync_WhenCompanyNotFound_ReturnsNotFoundError()

// Update
UpdateCompanyForTenantAsync_WithValidName_ReturnsSuccessResult()
UpdateCompanyForTenantAsync_WhenCompanyNotFound_ReturnsNotFoundError()

// Delete
DeleteCompanyForTenantAsync_WithValidData_ReturnsSuccessResult()
DeleteCompanyForTenantAsync_WithValidData_MarksCompanyAsDeleted()

// Collections
GetCompanyTypeaheadListForTenantAsync_WithValidParameters_ReturnsSuccessResult()
GetPaginatedCompaniesForTenantAsync_WithInvalidPage_UsesDefaultPage()
```
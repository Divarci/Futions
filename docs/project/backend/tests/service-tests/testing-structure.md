## Project Structure

Service tests follow **operation folder** organization:

**Example structure:**
```
api/test/Core/Futions.Crm.App.Tests/Services/
└── Organisations/
    └── Companies/
        ├── CompanyServiceTests.cs                              # Base class + setup + helpers
        ├── Create/
        │   ├── CreateCompanyForTenant/
        │   │   ├── CompanyServiceTests.CreateCompanyForTenant.Success.cs
        │   │   └── CompanyServiceTests.CreateCompanyForTenant.Failure.cs
        │   └── RestoreOrAddPersonToCompanyForTenantAsync/# ⚠️ NESTED OPERATION
        │       ├── CompanyServiceTests.RestoreOrAddPersonToCompanyForTenantAsync.Success.cs
        │       └── CompanyServiceTests.RestoreOrAddPersonToCompanyForTenantAsync.Failure.cs
        ├── Update/
        │   ├── UpdateCompanyForTenant/
        │   │   ├── CompanyServiceTests.UpdateCompanyForTenant.Success.cs
        │   │   └── CompanyServiceTests.UpdateCompanyForTenant.Failure.cs
        │   └── UpdateCompanyPersonForTenantAsync/# ⚠️ NESTED OPERATION
        │       ├── CompanyServiceTests.UpdateCompanyPersonForTenantAsync.Success.cs
        │       └── CompanyServiceTests.UpdateCompanyPersonForTenantAsync.Failure.cs
        ├── Delete/
        │   ├── DeleteCompanyForTenant/
        │   │   ├── CompanyServiceTests.DeleteCompanyForTenant.Success.cs
        │   │   └── CompanyServiceTests.DeleteCompanyForTenant.Failure.cs
        │   └── RemovePersonFromCompanyForTenantAsync/ # ⚠️ NESTED OPERATION
        │       ├── CompanyServiceTests.RemovePersonFromCompanyForTenantAsync.Success.cs
        │       └── CompanyServiceTests.RemovePersonFromCompanyForTenantAsync.Failure.cs
        ├── Single/
        │   └── CompanyServiceTests.GetCompanyByIdForTenant.cs  # ⚠️ SINGLE FILE
        └── Collections/
            ├── GetPaginatedCompaniesForTenant/
            │   └── CompanyServiceTests.GetPaginatedCompaniesForTenant.cs  # ⚠️ SINGLE FILE
            └── GetCompanyTypeaheadList/
            │   └── CompanyServiceTests.GetCompanyTypeaheadList.cs # ⚠️ SINGLE FILE         
            └── GetPersonCompaniesTypeaheadListForTenantAsync/# ⚠️ NESTED OPERATION
                └── CompanyServiceTests.GetPersonCompaniesTypeaheadListForTenantAsync.cs # ⚠️ SINGLE FILE         
```

### Structure Rules

**CRITICAL:** Since partial classes in each service represent a folder, unit tests for that service's methods should be created inside the folder with the same name **(NESTED OPERATION)**

1. **CRUD Operations (Create, Update, Delete, Single)**: Split into Success and Failure files
   - **ONLY Success.cs and Failure.cs are allowed**
   - ❌ **NEVER** create files named Additional.cs, PropertyVerification.cs, or any other custom names
   - ✅ All success tests go in Success.cs (including property verification, repository calls, commit verification)
   - ✅ All failure tests go in Failure.cs (including NotFound, ValidationError, and all error scenarios)
   
2. **Collections Operations**: Keep in SINGLE files (all tests together)

3. **Method-Level Folders**: Each service method gets its own folder

4. **Base Class**: Contains all mock setup and helper methods

**Rationale for Collections Pattern**:
- Collection tests are simpler (mostly happy path)
- Less complex validation scenarios
- Easier to review all tests together
- Reduces file overhead

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

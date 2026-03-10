## Test Naming Conventions

**Pattern**: `{MethodUnderTest}_{Scenario}_{ExpectedResult}`

## Naming Guidelines

### DO ✅

1. **Use descriptive names that explain the scenario:**
   ```csharp
   Create_WithValidData_ReturnsSuccessResult()  // Clear intent
   ```

2. **Include the state/condition being tested:**
   ```csharp
   Activate_WhenPending_ChangesStatusToActive()  // Clear precondition
   UpdateName_WhenEntityIsDeleted_ReturnsFailureResult()  // State check
   ```

3. **Be specific about the expected outcome:**
   ```csharp
   Create_WithNullName_ReturnsValidationError()  // Specific error type
   Delete_WhenAlreadyDeleted_ReturnsConflictError()  // Specific error type
   ```
4. **Use action verbs for the expected result:**
   ```csharp
   Create_GeneratesUniqueId()
   Delete_SetsIsDeletedToTrue()
   Activate_ChangesStatusToActive()
   ```

### DON'T ❌

1. **Don't use vague names:**
   ```csharp
   Test1()  // ❌ What does this test?
   CreateTest()  // ❌ Tests what scenario?
   TestWithInvalidData()  // ❌ Which method? What kind of invalid data?
   ```

2. **Don't use abbreviations or acronyms:**
   ```csharp
   Crt_ValidData_Success()  // ❌ Unclear
   Create_VD_RetSucc()  // ❌ Cryptic
   ```

3. **Don't mix naming patterns:**
   ```csharp
   // ❌ Inconsistent - pick one pattern
   Create_WithValidData_ReturnsSuccessResult()
   UpdateName_ReturnsSuccess_WhenValidName()
   WhenDeleted_Delete_ReturnsConflict()
   ```

4. **Don't include "Test" in the name (it's already in a test class):**
   ```csharp
   TestCreate_WithValidData()  // ❌ Redundant
   Create_Test_ValidData()  // ❌ Redundant
   ```

---

## Pattern Variations

### When Testing Multiple Related Scenarios

**Group by common condition:**
```csharp
// All validation scenarios
Create_WithNullName_ReturnsValidationError()
Create_WithEmptyName_ReturnsValidationError()
Create_WithTooLongName_ReturnsValidationError()

// All state-related scenarios
UpdateName_WhenActive_UpdatesSuccessfully()
UpdateName_WhenDeleted_ReturnsFailureResult()
UpdateName_WhenPending_ReturnsFailureResult()
```

### When Testing Edge Cases

**Be explicit about the edge case:**
```csharp
IncreaseUserLimitBy_ToExactlyMaxInt_Succeeds()  // Boundary value
```

### When Testing Business Rules

**Include the business rule in the name:**
```csharp
AddUser_WhenLimitReached_ReturnsConflictError()
DeleteCompany_WhenHasActivePeople_ReturnsConflictError()
DecreaseUserLimit_BelowCurrentUserCount_ReturnsConflictError()
AddSubscription_WhenAlreadyHasActiveSubscription_ReturnsConflictError()
```
**Remember:** Good test names are documentation. Someone should be able to understand what the test does without reading the implementation.

## Create Operation

**CRITICAL:** The following scenarios are baseline requirements and must be implemented in all tests unless the service has unique behaviour. If the entity includes additional domain-specific logic, corresponding test cases should also be added.

**Success Tests** - Typical scenarios:
- `WithValidData_ReturnsEntityAndSuccessResult` - Verify successful creation
- `WithValidData_CallsRepositoryCreate` - Verify repository called
- `WithValidData_CommitsTransaction` - Verify commit called

**Failure Tests** - Typical scenarios:

**CRITICAL**: Every failure tests must verify BOTH repository call (Times.Never) AND commit (Times.Never) at the end. Do not create separate test for them.

- `WithAllInvalidInputs_ReturnsValidationError` - Validation failure scenario
  - **NOTE**: If domain checks entity state BEFORE validation, you may get `Conflict` instead of `ValidationError`
  - Study your domain entity to understand check ordering
  - Adjust expected ErrorType accordingly
    
## Understanding Domain Complexity

**CRITICAL**: Domain entities may have complex business rules that affect test scenarios.

### Common Domain Checks (in order of execution):
1. **State checks** - `IsPending`, `IsDeleted`, `IsActive`
   - These typically return `Conflict` errors
   - Happen BEFORE validation
   
2. **Relationship checks** - Active subscriptions, user limits, foreign keys
   - May return `Conflict` or `NotFound` errors
   - Happen BEFORE or DURING validation
   
3. **Validation checks** - Required fields, formats, ranges
   - Return `ValidationError`
   - Happen LAST

### Impact on Tests:
- `WithAllInvalidInputs` might return `Conflict` if state check fails first
- Domain checks may prevent repository from being called even with valid input
- Entity state affects which tests are needed

**Recommendation**: 
1. Study your domain entity implementation first
2. Understand the order of checks
3. Adjust expected ErrorTypes accordingly
4. Add state-specific test scenarios where needed

## Complete Failure Test Example

**CRITICAL RULE**: Every failure test is ONE test method that verifies THREE things:
1. The correct error result is returned
2. Repository method is NOT called (Times.Never)
3. Commit is NOT called (Times.Never)

```csharp
[Fact]
public async Task Create{Entity}Async_When{Entity}NotFound_ReturnsNotFoundError()
{
    // Arrange
    var tenantId = GetValidTenantId();
    var model = CreateValidModel();
    
    _repositoryMock
        .Setup(r => r.Get...Async(tenantId, It.IsAny<CancellationToken>()))
        .ReturnsAsync((Entity?)null);

    // Act
    Result result = await _sut.Create{Entity}Async(tenantId, model, CancellationToken.None);

    // Assert - ALL THREE verifications in the SAME test
    AssertFailureResult(result, ErrorType.NotFound);                                      // 1. Error result
    _repositoryMock.Verify(r => r.Update(It.IsAny<Entity>()), Times.Never);              // 2. Repository not called
    _unitOfWorkMock.Verify(u => u.CommitAsync(It.IsAny<CancellationToken>()), Times.Never); // 3. Commit not called
}
```

**Remember**: Times.Never verifications ensure the service properly short-circuits on errors and doesn't waste resources or cause unintended side effects. These verifications are ALWAYS done together in the SAME test method.

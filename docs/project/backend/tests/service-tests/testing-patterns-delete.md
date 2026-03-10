
## Delete Operation

**CRITICAL:** The following scenarios are baseline requirements and must be implemented in all tests unless the service has unique behaviour. If the entity includes additional domain-specific logic, corresponding test cases should also be added.

**Success Tests**:
- `WithValidData_ReturnsSuccessResult` - Status: NoContent
- `WithValidData_MarksEntityAsDeleted` - IsDeleted = true
- `WithValidData_CallsRepositoryUpdate` - Repository.Update() called
- `WithValidData_CommitsTransaction` - Commit called

**Failure Tests**:

**CRITICAL**: Every failure tests must verify BOTH repository call (Times.Never) AND commit (Times.Never) at the end. Do not create separate test for them.

- `When{Entity}NotFound_ReturnsNotFoundError`
  - Replace `{Entity}` with your actual entity name
  - Example: `WhenCompanyNotFound_ReturnsNotFoundError`, `WhenUserNotFound_ReturnsNotFoundError`
 
**Domain-Specific Business Rule Failure Tests** - **VARIABLE BY DOMAIN LOGIC**

**Analysis Required**:
- Read your entity's Delete() domain method to identify ALL business rules
- Each business rule = 1-3 tests (depends on implementation flow)
- Error types vary: Conflict, BusinessRule, Validation, etc.
- Test names should clearly describe the blocking condition

## Understanding Delete Behavior

**Hard Delete vs Soft Delete**:
- **Soft Delete**: Sets `IsDeleted = true`, calls `Update()`
- **Hard Delete**: Removes from collection, may not call repository at all

**Business Rule Placement**:
- Rules in SERVICE: Check before calling repository → Repository never called on failure
- Rules in DOMAIN ENTITY: Entity method called → Entity state may be modified even on failure

**Recommendation**: 
1. Check your domain entity's Delete() method implementation
2. Determine if rules are checked before or during deletion
3. Only add conditional tests if they apply to your implementation

## Complete Failure Test Example
W
```csharp
[Fact]
public async Task Delete{Entity}Async_When{Entity}NotFound_ReturnsNotFoundError()
{
    // Arrange
    var tenantId = GetValidTenantId();
    var entityId = GetValidEntityId();
    
    _repositoryMock
        .Setup(r => r.Get...Async(tenantId, It.IsAny<CancellationToken>()))
        .ReturnsAsync((Entity?)null);

    // Act
    Result result = await _sut.Delete{Entity}Async(tenantId, entityId, CancellationToken.None);

    // Assert - ALL THREE verifications in the SAME test
    AssertFailureResult(result, ErrorType.NotFound);                                      // 1. Error result
    _repositoryMock.Verify(r => r.Update(It.IsAny<Entity>()), Times.Never);              // 2. Repository not called
    _unitOfWorkMock.Verify(u => u.CommitAsync(It.IsAny<CancellationToken>()), Times.Never); // 3. Commit not called
}
```

**Remember**: Times.Never verifications ensure the service properly short-circuits on errors. These verifications are ALWAYS done together in the SAME test method.

## Update Operation

**CRITICAL:** The following scenarios are baseline requirements and must be implemented in all tests unless the service has unique behaviour. If the entity includes additional domain-specific logic, corresponding test cases should also be added.

**Success Tests**:
- `WithValidX_ReturnsSuccessResult` - Status: NoContent
- `WithValidX_UpdatesEntityX` - Entity property updated
- `WithValidData_CallsRepositoryUpdate` - Repository.Update() called
- `WithValidData_CommitsTransaction` - Commit called

**Failure Tests**:

**CRITICAL**: Every failure tests must verify BOTH repository call (Times.Never) AND commit (Times.Never) at the end. Do not create separate test for them.

- `WhenEntityNotFound_ReturnsNotFoundError`
- `WithInvalidData_ReturnsValidationError`

- `WithEmptyModel_StillCommitsTransaction`
  - This is a SUCCESS scenario (partial update with no changes is valid)
  - Verify commit IS called: `Times.Once`

- `WithEmptyModel_TreatsAsNoUpdate`
  - Verify entity properties remain unchanged when model is empty
  - Example: `entity.Name.Should().Be(originalName);`
  - This confirms domain correctly handles null/empty values

## Understanding Update Behavior

**Partial Updates**:
- Update operations typically allow partial updates (only some properties)
- Null/empty values in model usually mean "don't update this property"
- Domain entity should preserve existing values for null properties

**Property Verification**:
- After calling service, verify entity state directly
- Don't just check return value - check entity was actually modified
- This catches cases where service succeeds but domain doesn't update

**Recommendation**:
1. Create separate success test for each updatable property
2. Verify both return result AND entity state
3. Test empty model to ensure no unintended changes

## Complete Failure Test Example

```csharp
[Fact]
public async Task Update{Entity}Async_When{Entity}NotFound_ReturnsNotFoundError()
{
    // Arrange
    var tenantId = GetValidTenantId();
    var model = CreateValidUpdateModel();
    
    _repositoryMock
        .Setup(r => r.Get...Async(tenantId, It.IsAny<CancellationToken>()))
        .ReturnsAsync((Entity?)null);

    // Act
    Result result = await _sut.Update{Entity}Async(tenantId, model, CancellationToken.None);

    // Assert - ALL THREE verifications in the SAME test
    AssertFailureResult(result, ErrorType.NotFound);                                      // 1. Error result
    _repositoryMock.Verify(r => r.Update(It.IsAny<Entity>()), Times.Never);              // 2. Repository not called
    _unitOfWorkMock.Verify(u => u.CommitAsync(It.IsAny<CancellationToken>()), Times.Never); // 3. Commit not called
}
```

**Remember**: Times.Never verifications ensure the service properly short-circuits on errors. These verifications are ALWAYS done together in the SAME test method.

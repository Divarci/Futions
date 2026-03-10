### ✅ DO

1. **Always verify Result pattern returns**
2. **Always test entity state changes**
3. **Always use correct ErrorType**
4. **Always test ALL validations**
5. **Always test business rules**
6. **Always use helper methods**
7. **Always test immutability for value objects**
8. **Always investigate domain business logic**
9. **Always follow the AAA pattern**
10. **Always use descriptive test names**
11. **Always test validations** : Only failure tests must be done 

---

### ❌ DON'T

1. **Don't skip entity state verification**:
   - ❌ `result.IsSuccess.Should().BeTrue();` (incomplete)
   - ✅ Verify both Result AND entity state

2. **Don't test infrastructure concerns**:
   - ❌ Database connections
   - ❌ Repository implementations
   - ❌ External services
   - ✅ Pure domain logic only

3. **Don't use mocking in domain tests**:
   - Domain tests should NEVER use Moq or other mocking frameworks
   - Domain entities have no dependencies to mock
   - If you need mocking, you're testing the wrong layer

4. **Don't forget edge cases**:
   - Test boundary values (max length, min value, etc.)
   - Test null inputs where applicable
   - Test empty collections
   - Test state transitions

5. **Don't test multiple concerns in one test**:
   - ❌ One test checking creation, update, and delete
   - ✅ Separate tests for each operation and scenario

6. **Don't use magic numbers or strings**:
   - ❌ `Create_WithTooLongName_ReturnsError() { var name = new string('x', 301); }`
   - ✅ Use constants or helper methods: `CreateStringOfLength(301)`

7. **Don't ignore validation order**:
   - Domain entities may check state BEFORE validation
   - Understand the order of checks in your entity
   - Adjust expected ErrorTypes accordingly

8. **Don't duplicate assertion logic**:
   - ❌ Repeating `result.IsSuccess.Should().BeTrue()` everywhere
   - ✅ Create assertion helpers: `AssertSuccessResult(result)`

9. **Don't test private methods**:
   - Always test through public interface
   - Private methods are implementation details

10. **Don't skip failure scenarios**:
    - Happy paths alone are insufficient
    - Test ALL validation failures.( NOT in entity tests as they will have their own tests)
    - Test ALL business rule violations
    - Test ALL state conflicts
      
11. **Don't test success use cases for validation methods**
---

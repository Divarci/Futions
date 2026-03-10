## Testing Critical Rules

### ✅ DO

1. **Always verify UnitOfWork.CommitAsync()**:
   - Write operations (Create, Update, Delete): `Times.Once`
   - Read operations (Single, Collections): `Times.Never`

2. **Always verify repository method calls**

3. **Always test entity state changes**:
   - After Create: Verify properties
   - After Update: Verify updated properties
   - After Delete: Verify `IsDeleted = true`

4. **Always test status codes**:
   - Create: `HttpStatusCode.Created`
   - Update/Delete: `HttpStatusCode.NoContent`
   - Single/Collections: `HttpStatusCode.OK`

5. **Use assertion helpers** to reduce duplication

6. **Use [Theory] with [InlineData]** for multiple values

7. **Always make one test for domain validations**
   - Verify Error type only. Error type must be ErrorType.ValidationError

8. **Critical : Always resolve domain business logic**
   - Read all required domain files.
   - Extract domain business rules and create required helper methods.
   - You must have an ability to create Helper method for every single entity.

### ❌ DON'T

1. **Don't skip transaction verification** - Critical for data integrity
2. **Don't test domain logic** - That belongs in domain tests
3. **Don't use magic numbers** - Use named constants
4. **Don't test multiple concerns** - One behavior per test
5. **Don't forget failure scenarios**
6. **Don't use verbose "because" unless necessary**
7. **Never skip reading and resolving domain business** - It is crucial for generating helper methods.
8. **Never create separate task to verify:** `When{Condition}_DoesNotCallRepository` - `When{Condition}_DoesNotCommit`

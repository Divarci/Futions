### Single Query Operation - SINGLE FILE

**CRITICAL:** The following scenarios are baseline requirements and must be implemented in all tests unless the service has unique behaviour. If the entity includes additional domain-specific logic, corresponding test cases should also be added.

**Scenarios**:
- `WithValidId_ReturnsSuccessResult` 
- `WithValidId_ReturnsEntityWithCorrectData`
- `WithValidId_CallsRepository` 
- `DoesNotCommitTransaction`

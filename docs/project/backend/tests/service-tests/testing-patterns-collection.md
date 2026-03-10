## Collections

**CRITICAL:** The following scenarios are baseline requirements and must be implemented in all tests unless the service has unique behaviour. If the entity includes additional domain-specific logic, corresponding test cases should also be added.

### Pagination (Collection) - SINGLE FILE

**Scenarios** (All in ONE file):
- `WithValidParameters_ReturnsSuccessResult`
- `WithInvalidPage_UsesDefaultPage` - Use [Theory]
    - [InlineData(null)]
    - [InlineData(0)]
    - [InlineData(-1)]
- `WithInvalidPageSize_UsesDefaultPageSize` - Use [Theory]
    - [InlineData(null)]
    - [InlineData(0)]
    - [InlineData(-1)]
- `WithFilter_CallsRepositoryWithFilter`
- `WithNullSortBy_UsesDefaultSort`
- `WithEmptyResult_ReturnsSuccessWithEmptyArray`
- `DoesNotCommitTransaction` - ⚠️ CRITICAL

### Typeahead (Collections) - SINGLE FILE

**Scenarios**:
- `WithValidParameters_ReturnsSuccessResult`
- `WithFilter_CallsRepositoryWithFilter`
- `DoesNotCommitTransaction` - ⚠️ CRITICAL
- `WithEmptyResult_ReturnsSuccessWithEmptyArray`

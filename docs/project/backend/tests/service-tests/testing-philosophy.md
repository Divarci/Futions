## Testing Philosophy

### Core Principles

1. **Test Service Orchestration** - Services coordinate domain entities, repositories, and transactions
2. **Isolation Through Mocking** - Mock external dependencies (repositories, unit of work)
3. **Fast Execution** - Tests complete quickly without database or external services
4. **Readable** - Tests document service behavior and workflows
5. **Maintainable** - Easy to update when service logic changes
6. **Comprehensive** - Cover success paths, edge cases, validation, and error scenarios

### What to Test at Service Layer

- Service method orchestration logic
- Repository interaction patterns (verify method calls with correct parameters)
- Transaction boundaries (UnitOfWork commits)
- Domain model validation integration
- Business workflow coordination
- Result pattern usage and status codes
- Default parameter handling (null/invalid parameters)


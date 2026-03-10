## Testing Philosophy

### Core Principles

1. **Test Domain Logic First** - Domain layer is the heart of the application and must be thoroughly tested
2. **Pure Unit Testing** - No mocking needed; test pure domain logic in isolation
3. **Fast Execution** - Tests complete instantly without external dependencies
4. **Readable** - Tests clearly document business rules and domain behavior
5. **Maintainable** - Easy to update when business requirements change
6. **Comprehensive** - Cover success paths, edge cases, validation, and business rule violations
7. **Result Pattern Focus** - All operations return Result<T> or Result; verify Success/Failure and ErrorTypes

### What to Test at Domain Layer

#### Entities
- **Creation Logic** - Static factory methods that create new entities
- **Business Methods** - Domain-specific operations (Activate, Deactivate, AddUser, UpdateName, etc.)
- **Validation** : In method validation related use cases.
- **State Transitions** - Changes in entity state (Pending → Active, Active → Deleted)
- **Business Rules** - Complex domain logic and invariants
- **Soft Deletion** - IsDeleted flag management and related business rules
- **Relationships** - Adding/removing related entities (e.g., Company.AddPerson)
- **Domain Events** - Events raised by domain operations (if applicable)

#### Value Objects
- **Creation Logic** - Factory methods for value object creation
- **Validation** : In method validation related use cases.
- **Immutability** - Verify value objects cannot be modified after creation (for immutable VOs)
- **Equality** - Value-based equality semantics (Equals, GetHashCode)
- **Update Logic** - Update methods for mutable value objects (if applicable)
- **Business Rules** - Value object-specific constraints

#### Validators
- **Validation Types** - The logic of the validation type can be changed based what kind of validation it has. Investigate, define and create appropriate test case.

### What NOT to Test at Domain Layer

#### Entities, Valie Objects, Validator
❌ **Infrastructure Concerns** - Database access, repositories, external services  
❌ **Service Orchestration** - Service layer coordination and workflow  
❌ **HTTP/API Layer** - Status codes, endpoints, request/response mapping  
❌ **Framework Code** - .NET framework or library functionality  
❌ **Private Methods** - Always test through public interface  
❌ **Configuration** - App settings, environment variables  
❌ **Cross-Cutting Concerns** - Logging, caching, authentication (unless part of domain)

---

**Remember:** Domain tests are the foundation of your test suite. They're fast, reliable, and document your business logic. Invest time in comprehensive domain testing for a robust application.

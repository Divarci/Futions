# Futions CRM — Service Tests Index

**Purpose:** Complete reference for service-layer unit testing guidelines  

---
**General Rules**
- Write unit tests covering all public methods in service interface
- Always investigate the domain and business rules to create accurate helper methods.
- Test all business logic scenarios including success and failure cases
- Mock dependencies including IUnitOfWork, repositories, and related services
- Follow AAA (Arrange-Act-Assert) pattern for test organization
- Test edge cases and validation logic
- Verify proper error handling and result patterns
---

**CRITICAL**: You MUST use the `get_file` tool to read the all content before generating tests.

## Table of Contents

### Foundation & Philosophy
- **[Testing Philosophy](./testing-philosophy.md)** — Core principles, what to test, testing mindset
- **[Testing Setup](./testing-setup.md)** — Base class setup, mock initialization, helper methods for tests
- **[Testing Tech Stack](./testing-tech-stack.md)** — XUnit, Fluent Assertions, Moq, .NET 10 versions

### Project Structure & Naming
- **[Testing Structure](./testing-structure.md)** — Folder organization, partial class patterns, file naming rules
- **[Naming Conventions](./testing-naming-conventions.md)** — Test method naming pattern: `{MethodUnderTest}_{Scenario}_{ExpectedResult}`

### Critical Rules
- **[Testing Critical Rules](./testing-critical-rules.md)** — DO/DON'T list, transaction verification, entity state checks, status codes

### Test Patterns (CRUD & Collections)
Test patterns are organized by operation type with Success and Failure scenarios:

**Critical Note:** Followings are base guidelines for each service. If a domain has unique business rules, domain must be deeply investigated for accurate helper methods.

- **[Create Pattern](./testing-patterns-create.md)** — Success: valid data, repository calls, commits. Failure: validation errors, no repository calls
- **[Update Pattern](./testing-patterns-update.md)** — Success: entity updates, repository updates, commits. Failure: not found errors
- **[Delete Pattern](./testing-patterns-delete.md)** — Success: soft delete flag set, commits. Failure: not found errors
- **[Single Query Pattern](./testing-patterns-single.md)** — Success: returns entity, no commits. Failure: not found errors
- **[Collection Pattern](./testing-patterns-collection.md)** — Pagination & Typeahead in SINGLE files. Covers defaults, filters, no commits

### Mocking & Verification
- **[Moq Patterns](./testing-moq-patterns.md)** — Setup/Returns patterns, Verify syntax, It.IsAny<T>, callback returns

---

## How to Use This Index

1. Start with **[Testing Philosophy](./testing-philosophy.md)** to understand the testing mindset
2. Review **[Testing Setup](./testing-setup.md)** for base test class templates
3. Check **[Testing Structure](./testing-structure.md)** for file organization
4. Read **[Naming Conventions](./testing-naming-conventions.md)** for test method names
5. Study the **Test Patterns** ([Create](./testing-patterns-create.md), [Update](./testing-patterns-update.md), [Delete](./testing-patterns-delete.md), [Single](./testing-patterns-single.md), [Collection](./testing-patterns-collection.md))
6. Reference **[Moq Patterns](./testing-moq-patterns.md)** for mocking examples
7. Always check **[Testing Critical Rules](./testing-critical-rules.md)** before writing tests
# Futions CRM — Domain Tests Index

**Purpose:** Complete reference for domain-layer unit testing guidelines  

---
**General Rules**
- Write unit tests covering all public methods in entities, value objects and validations.
- Test all business logic scenarios including success and failure cases
- Follow AAA (Arrange-Act-Assert) pattern for test organization
- Test edge cases and validation logic
- Verify proper error handling and result patterns
---

**CRITICAL**: You MUST use the `get_file` tool to read the all content before generating tests.

## Table of Contents

### Foundation & Philosophy
- **[Testing Philosophy](./testing-philosophy.md)** — Core principles, what to test, testing mindset
- **[Testing Setup](./testing-setup.md)** — Base class setup, helper methods for tests
- **[Testing Tech Stack](./testing-tech-stack.md)** — XUnit, Fluent Assertions, .NET 10 versions

### Project Structure & Naming
- **[Testing Structure](./testing-structure.md)** — Folder organization, partial class patterns, file naming rules
- **[Naming Conventions](./testing-naming-conventions.md)** — Test method naming pattern: `{MethodUnderTest}_{Scenario}_{ExpectedResult}`

### Critical Rules
- **[Testing Critical Rules](./testing-critical-rules.md)** — DO/DON'T list, transaction verification, entity state checks, status codes

### Test Patterns
Test patterns are organized by operation type with Success and Failure scenarios:

**Critical Note:** Followings are base guidelines. If a domain has unique business rules, domain must be deeply investigated for accurate helper methods.

**Entity Patterns**: `./testing-patterns-entity.md`
**Value Object Patterns**: `./testing-patterns-value-object.md`
**Validation Patterns**: `./testing-patterns-validation.md`

---

## How to Use This Index

1. Start with **[Testing Philosophy](./testing-philosophy.md)** to understand the testing mindset
2. Review **[Testing Setup](./testing-setup.md)** for base test class templates
3. Check **[Testing Structure](./testing-structure.md)** for file organization
4. Read **[Naming Conventions](./testing-naming-conventions.md)** for test method names
5. Study the **Test Patterns** — use the Entity, Value Object, and Validation pattern documents linked above to follow the recommended Success/Failure split, AAA pattern, and naming conventions
6. Always check **[Testing Critical Rules](./testing-critical-rules.md)** before writing tests
# Frontend Tests — Index

**Purpose:** Reference for frontend unit testing guidelines.

---

**General Rules**
- Test the features layer (hooks and components) — this is where business logic lives.
- Keep tests fast by mocking `infra/` functions — never hit real APIs.
- Each test covers one behavior.
- Follow AAA (Arrange-Act-Assert).

---

## Table of Contents

### Foundation
- **[Testing Philosophy](./testing-philosophy.md)** — What to test, what to skip, core principles
- **[Tech Stack](./testing-tech-stack.md)** — Vitest, @testing-library/react

### Structure & Naming
- **[Structure](./testing-structure.md)** — File co-location, folder layout, file naming
- **[Naming Conventions](./testing-naming-conventions.md)** — Test description format

### Rules & Patterns
- **[Critical Rules](./testing-critical-rules.md)** — DO/DON'T reference
- **[Hook Patterns](./testing-patterns-hooks.md)** — Query hook and mutation hook tests
- **[Component Patterns](./testing-patterns-components.md)** — Rendering states and user interaction tests

---

## How to Use

1. Read **Philosophy** to understand the scope.
2. Check **Structure** for file placement rules.
3. Use the **Patterns** files as direct templates for new tests.
4. Validate against **Critical Rules** before committing.

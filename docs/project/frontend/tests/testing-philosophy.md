# Testing Philosophy

## Core Principles

1. **Features layer first** — Hooks and components contain business logic. Test them.
2. **Mock at the boundary** — Mock `infra/` functions, not HTTP. Keep tests fast and isolated.
3. **One behavior per test** — Each test verifies one specific outcome.
4. **User-visible behavior** — Test what the component renders and what it does, not how it does it.
5. **AAA** — Arrange, Act, Assert. One clear block each.

---

## What to Test

| Layer | What | Why |
|---|---|---|
| `features/hooks/` | Data fetching states, mutation calls, revalidation | Core async business logic |
| `features/components/` | Loading / error / data rendering, user interactions | Visible behavior |
| `core/utils/` | Input → output | Pure functions with no dependencies |

---

## What NOT to Test

| Skip | Reason |
|---|---|
| `infra/` API functions | Thin Axios wrappers — no business logic to verify |
| `app/` pages | Thin wrappers around feature components |
| Tailwind CSS classes | Styling is not behavior |
| SWR internals | Library responsibility, not yours |
| `core/components/` primitives | Implicitly covered when testing consuming feature components |

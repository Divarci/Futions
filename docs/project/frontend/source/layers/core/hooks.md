# Core — Hooks

Shared utility hooks used across multiple features. Contain no domain knowledge and no API calls.

---

## Rules

- No domain types or domain-specific logic.
- No API calls.
- Must be used by three or more unrelated features to justify living in `core/`.

---

## Examples

`useDebounce`, `usePagination`

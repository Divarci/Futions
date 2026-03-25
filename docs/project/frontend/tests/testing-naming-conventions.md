# Testing — Naming Conventions

---

## File Organization

Each test file uses `describe` for the subject and `it` for each behavior.

```typescript
describe("useGetTasks", () => {
    it("returns tasks when API call succeeds", ...)
    it("returns error when API call fails", ...)
});
```

---

## `it` Description Format

**Pattern**: `"{action/state} {expected outcome}"`

### DO ✅

```
"returns tasks when API call succeeds"
"returns error when API call fails"
"skips fetch when taskId is not provided"
"calls createTask with the provided model"
"revalidates tasks list after successful creation"
"exposes error when mutation fails"
"renders loading state while data is pending"
"renders error message when hook returns error"
"renders task list when data is available"
"calls onDelete when delete button is clicked"
```

### DON'T ❌

```
"test1"                  ← no meaning
"works correctly"        ← too vague
"useGetTasks test"       ← redundant with describe
"should return data"     ← drop "should" — use present tense
"it loads"               ← incomplete
```

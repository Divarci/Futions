# App — Error Boundaries and Loading States

Each route segment may define its own `error.tsx` and `loading.tsx` following Next.js conventions.

---

## `error.tsx`

Catches unhandled errors propagated from the React component tree below. It is the last-resort safety net for exceptions not caught at the feature level. Must be a Client Component (`"use client"`).

```typescript
// app/{domain}/error.tsx
"use client";

type ErrorProps = {
    error: Error;
    reset: () => void;
};

export default function {Entities}Error({ error, reset }: ErrorProps) {
    return (
        <div>
            <p>{error.message}</p>
            <button onClick={reset}>Try again</button>
        </div>
    );
}
```

---

## `loading.tsx`

Renders a Suspense fallback while the route segment suspends. Use it for route-level skeleton screens.

---

## Coordination with Feature-Level Error Handling

Feature-level loading and error states (SWR's `isLoading`, `error`) handle granular inline feedback. `error.tsx` and `loading.tsx` are the outer safety net — they exist for cases where nothing else caught the failure.

- Feature components handle their own loading and error states (granular, inline UI feedback).
- `loading.tsx` shows a route-level skeleton while the page suspends.
- `error.tsx` catches any unhandled error that escapes the feature component tree.

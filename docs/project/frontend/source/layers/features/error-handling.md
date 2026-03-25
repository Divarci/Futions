# Features — Error Handling

---

## Feature Level — SWR

Handle loading and error states in the consuming component using the values returned by the hook. Every component that calls a query hook must handle all three states: loading, error, and data.

```typescript
const { data, isLoading, error } = useGetTasks(filter);

if (isLoading) return <Spinner />;
if (error)     return <ErrorMessage message="Could not load tasks." />;
```

For mutations, use `onError` in `useSWRMutation` to react to failures (e.g., display a toast notification). Retry strategy is configured per-hook via SWR's `onErrorRetry` option.

---

## App Level — Coordination with `error.tsx` / `loading.tsx`

`error.tsx` and `loading.tsx` in `app/` are the **outer safety net** — they catch unhandled errors and Suspense fallbacks respectively. They complement, not replace, feature-level error handling.

- Feature components handle their own loading and error states (granular, inline UI feedback).
- `loading.tsx` shows a route-level skeleton while the page suspends.
- `error.tsx` catches any unhandled error that escapes the feature component tree.

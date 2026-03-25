# Testing — Critical Rules

---

## DO ✅

1. **Mock infra functions** — not Axios, not HTTP, not the network. Mock at the function level:
   ```typescript
   vi.mock("@/infra/tasks");
   vi.mocked(getTasks).mockResolvedValue(mockData);
   ```

2. **Test all three hook states** — `isLoading`, `error`, `data`. Never skip a state.

3. **Wrap SWR hooks in `SWRConfig`** with a fresh cache per test to prevent state leakage between tests:
   ```typescript
   const wrapper = ({ children }: { children: ReactNode }) => (
       <SWRConfig value={{ provider: () => new Map(), dedupingInterval: 0 }}>
           {children}
       </SWRConfig>
   );
   ```

4. **Mock the hook when testing a component** — never let the component reach real infra:
   ```typescript
   vi.mock("../hooks/useGetTasks");
   vi.mocked(useGetTasks).mockReturnValue({ data: ..., isLoading: false, error: undefined });
   ```

5. **Use `@testing-library` queries** — `getByRole`, `getByText`, `getByLabelText`. Never query by CSS class.

6. **Use `waitFor`** when testing async state changes in query hooks.

7. **Use `act`** when calling `trigger` on mutation hooks.

---

## DON'T ❌

1. **Don't test implementation details** — no assertions on internal state, refs, or private variables.
2. **Don't test infra API functions** — thin Axios wrappers with no logic.
3. **Don't test `app/` pages** — thin wrappers with nothing to verify.
4. **Don't use snapshot tests** — brittle, document nothing useful.
5. **Don't share SWR cache between tests** — always provide `provider: () => new Map()`.
6. **Don't test Tailwind CSS classes** — styling is not behavior.

# Testing — Hook Patterns

---

## Shared Wrapper

Define once per test file. Provides a fresh SWR cache for every test.

```typescript
import { SWRConfig }  from "swr";
import type { ReactNode } from "react";

const wrapper = ({ children }: { children: ReactNode }) => (
    <SWRConfig value={{ provider: () => new Map(), dedupingInterval: 0 }}>
        {children}
    </SWRConfig>
);
```

---

## Query Hook

```typescript
// features/tasks/hooks/useGetTasks.test.ts
import { renderHook, waitFor } from "@testing-library/react";
import { getTasks }            from "@/infra/tasks";
import { useGetTasks }         from "./useGetTasks";

vi.mock("@/infra/tasks");

describe("useGetTasks", () => {

    it("returns tasks when API call succeeds", async () => {
        // Arrange
        const tasks = [{ taskId: "1", title: "Task A" }];
        vi.mocked(getTasks).mockResolvedValue(tasks);

        // Act
        const { result } = renderHook(() => useGetTasks({}), { wrapper });

        // Assert
        await waitFor(() => expect(result.current.data).toEqual(tasks));
    });

    it("returns error when API call fails", async () => {
        // Arrange
        vi.mocked(getTasks).mockRejectedValue(new Error("Network error"));

        // Act
        const { result } = renderHook(() => useGetTasks({}), { wrapper });

        // Assert
        await waitFor(() => expect(result.current.error).toBeDefined());
    });

    it("skips fetch when key is null", () => {
        // useGetTask passes null key when taskId is empty
        const { result } = renderHook(() => useGetTask(""), { wrapper });

        expect(result.current.isLoading).toBe(false);
        expect(result.current.data).toBeUndefined();
    });
});
```

---

## Mutation Hook

```typescript
// features/tasks/hooks/useCreateTask.test.ts
import { renderHook, act } from "@testing-library/react";
import { createTask }      from "@/infra/tasks";
import { useCreateTask }   from "./useCreateTask";

vi.mock("@/infra/tasks");

describe("useCreateTask", () => {

    it("calls createTask with the provided model", async () => {
        // Arrange
        const model = { title: "New Task", description: null, dueDate: null, tagIds: [] };
        vi.mocked(createTask).mockResolvedValue({ taskId: "1", ...model });

        const { result } = renderHook(() => useCreateTask(), { wrapper });

        // Act
        await act(() => result.current.trigger(model));

        // Assert
        expect(createTask).toHaveBeenCalledWith(model);
    });

    it("exposes error when mutation fails", async () => {
        // Arrange
        vi.mocked(createTask).mockRejectedValue(new Error("Server error"));

        const { result } = renderHook(() => useCreateTask(), { wrapper });

        // Act
        await act(() =>
            result.current.trigger({ title: "x", description: null, dueDate: null, tagIds: [] })
        );

        // Assert
        expect(result.current.error).toBeDefined();
    });
});
```

---

## Rules

- Always use the `wrapper` with a fresh SWR cache provider.
- Use `waitFor` for query hooks — data arrives asynchronously.
- Use `act` when calling `trigger` on mutation hooks.
- Verify the infra function was called with the correct arguments (`toHaveBeenCalledWith`).

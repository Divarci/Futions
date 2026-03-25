# Features — Hooks

SWR hooks that wrap `infra/` API call functions. Each hook wraps exactly one function.

---

## Query Hook — Collection

```typescript
// features/tasks/hooks/useGetTasks.ts
import useSWR from "swr";
import { getTasks } from "@/infra/tasks";
import type { TaskFilterParams } from "../types/task.types";

export function useGetTasks(filter: TaskFilterParams) {
    return useSWR(["tasks", filter], () => getTasks(filter));
}
```

---

## Query Hook — Single

```typescript
// features/tasks/hooks/useGetTask.ts
import useSWR from "swr";
import { getTask } from "@/infra/tasks";

export function useGetTask(taskId: string) {
    return useSWR(
        taskId ? ["tasks", taskId] : null,
        () => getTask(taskId),
    );
}
```

---

## Mutation Hook

```typescript
// features/tasks/hooks/useCreateTask.ts
import useSWRMutation from "swr/mutation";
import { useSWRConfig } from "swr";
import { createTask }   from "@/infra/tasks";
import type { TaskCreateModel } from "../types/task.types";

export function useCreateTask() {

    const { mutate } = useSWRConfig();

    return useSWRMutation(
        ["tasks"],
        (_key, { arg }: { arg: TaskCreateModel }) => createTask(arg),
        { onSuccess: () => mutate((key) => Array.isArray(key) && key[0] === "tasks", undefined, { revalidate: true }) },
    );
}
```

---

## Rules

- Each hook wraps exactly one `infra/` function — no combined calls.
- Hook names follow the pattern `use{Action}{Entity}` (e.g., `useGetTasks`, `useCreateTask`).
- SWR keys are arrays: `["{domain}", params]`. Pass `null` as key to conditionally skip fetching.
- Mutation hooks revalidate related queries in `onSuccess` via the global `mutate`.
- Hooks must never call `infra/` functions that belong to a different domain.

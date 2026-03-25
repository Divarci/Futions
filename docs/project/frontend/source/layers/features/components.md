# Features — Components

Feature-specific components that consume hooks to render domain data.

---

## Rendering Model

All components in `features/{domain}/components/` are **Client Components**. They consume SWR hooks which depend on React context, making the `"use client"` directive mandatory.

- Every component that uses a hook must declare `"use client"` at the top of the file.
- Do **not** create Server Components inside `features/components/` — Server Components that fetch data belong in `app/`.
- Mixing server and client rendering logic inside a single feature component is forbidden.

---

## Pattern

```typescript
// features/tasks/components/TaskList.tsx
"use client";

import { useGetTasks } from "../hooks/useGetTasks";
import { TaskCard }    from "./TaskCard";
import { Spinner }     from "@/core/components";
import type { TaskFilterParams } from "../types/task.types";

type TaskListProps = {
    filter: TaskFilterParams;
};

export function TaskList({ filter }: TaskListProps) {

    const { data, isLoading, error } = useGetTasks(filter);

    if (isLoading)
        return <Spinner />;

    if (error)
        return <p>Something went wrong.</p>;

    return (
        <ul>
            {data?.map(task => (
                <TaskCard key={task.taskId} task={task} />
            ))}
        </ul>
    );
}
```

---

## Rules

- Always handle `isLoading`, `error`, and `data` — never skip any state.
- Components never import from `infra/` directly — all data comes through hooks.
- Named exports only — no default exports.

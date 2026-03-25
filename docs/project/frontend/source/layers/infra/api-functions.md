# Infra — API Functions

One `.api.ts` file per domain. Each function performs exactly one HTTP call and returns the raw response data. No error handling, no state, no React.

---

## Pattern

```typescript
// infra/tasks/task.api.ts
import { httpClient } from "@/infra/http";
import type {
    TaskViewModel,
    TaskCreateModel,
    TaskUpdateModel,
    TaskFilterParams,
} from "@/features/tasks";

const BASE = "/api/v1/tasks";

export async function getTasks(filter: TaskFilterParams): Promise<TaskViewModel[]> {
    const response = await httpClient.get<TaskViewModel[]>(BASE, { params: filter });
    return response.data;
}

export async function getTask(taskId: string): Promise<TaskViewModel> {
    const response = await httpClient.get<TaskViewModel>(`${BASE}/${taskId}`);
    return response.data;
}

export async function createTask(model: TaskCreateModel): Promise<TaskViewModel> {
    const response = await httpClient.post<TaskViewModel>(BASE, model);
    return response.data;
}

export async function updateTask(taskId: string, model: TaskUpdateModel): Promise<TaskViewModel> {
    const response = await httpClient.patch<TaskViewModel>(`${BASE}/${taskId}`, model);
    return response.data;
}

export async function deleteTask(taskId: string): Promise<void> {
    await httpClient.delete(`${BASE}/${taskId}`);
}
```

---

## Rules

- One function per HTTP call — no multi-step operations.
- Functions return raw response data — no transformation, no mapping.
- No error handling here — that belongs in `features/` hooks.
- Function names mirror the HTTP operation: `get`, `create`, `update`, `delete`.

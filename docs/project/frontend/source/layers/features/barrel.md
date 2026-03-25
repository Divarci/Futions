# Features — Barrel Export

Every feature exposes its public API through a single `index.ts` barrel file. Files outside the feature always import from this barrel — never from internal paths.

---

## Pattern

```typescript
// features/tasks/index.ts
export { TaskList }      from "./components/TaskList";
export { TaskCard }      from "./components/TaskCard";
export { TaskForm }      from "./components/TaskForm";
export { useGetTasks }   from "./hooks/useGetTasks";
export { useGetTask }    from "./hooks/useGetTask";
export { useCreateTask } from "./hooks/useCreateTask";
export { useUpdateTask } from "./hooks/useUpdateTask";
export { useDeleteTask } from "./hooks/useDeleteTask";
export type {
    TaskViewModel,
    TaskCreateModel,
    TaskUpdateModel,
    TaskFilterParams,
    TaskStatus,
}                        from "./types/task.types";
```

---

## Rules

- Every public symbol (component, hook, type, action) must be re-exported from `index.ts`.
- Files outside the feature import from `@/features/{domain}` — never from deep paths like `@/features/tasks/hooks/useGetTasks`.
- Internal files within the same feature may use relative imports.

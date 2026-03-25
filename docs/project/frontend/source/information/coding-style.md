# Coding Style

This document establishes the code quality and consistency standards used across the frontend. All rules apply to every layer.

---

## Naming Conventions

### Files

| Element | Convention | Example |
|---|---|---|
| Component file | PascalCase | `TaskCard.tsx`, `TaskList.tsx` |
| Hook file | camelCase, `use` prefix | `useGetTasks.ts`, `useCreateTask.ts` |
| API file | camelCase, `.api.ts` suffix | `task.api.ts`, `tag.api.ts` |
| Type file | camelCase, `.types.ts` suffix | `task.types.ts`, `common.types.ts` |
| Utility file | camelCase, `.utils.ts` suffix | `date.utils.ts` |
| Constant file | camelCase, `.constants.ts` suffix | `api-routes.constants.ts` |
| Index/barrel file | `index.ts` | `features/tasks/index.ts` |

### TypeScript Identifiers

| Element | Convention | Example |
|---|---|---|
| Component (function) | PascalCase | `TaskCard`, `TaskList` |
| Hook (function) | camelCase, `use` prefix | `useGetTasks`, `useCreateTask` |
| Type | PascalCase | `TaskViewModel`, `TaskCreateModel` |
| Enum | PascalCase | `TaskStatus` |
| Enum value | PascalCase | `TaskStatus.Pending`, `TaskStatus.Completed` |
| Variable / parameter | camelCase | `taskId`, `createModel` |
| Constant (module-level) | SCREAMING_SNAKE_CASE | `API_BASE_URL` |
| Props type | PascalCase, `Props` suffix | `TaskCardProps`, `TaskListProps` |

### Async Functions

All async functions in hooks and API files are named to reflect their operation:

```
// hooks
useGetTasks(filter: TaskFilterParams)
useGetTask(taskId: string)
useCreateTask()
useUpdateTask()
useDeleteTask()

// api functions
getTasks(filter: TaskFilterParams): Promise<TaskViewModel[]>
getTask(taskId: string): Promise<TaskViewModel>
createTask(model: TaskCreateModel): Promise<TaskViewModel>
updateTask(taskId: string, model: TaskUpdateModel): Promise<TaskViewModel>
deleteTask(taskId: string): Promise<void>
```

---

## Exports

### Named Exports Only

All files use named exports. Default exports are only used where Next.js requires them (`page.tsx`, `layout.tsx`, `error.tsx`, `loading.tsx`).

```
// CORRECT
export function TaskCard({ task }: TaskCardProps) { ... }
export type { TaskViewModel };

// INCORRECT — no default exports in components or hooks
export default function TaskCard() { ... }
```

### Barrel Files (`index.ts`)

Each feature folder exposes a public API through its `index.ts`. Files outside the feature import only from the barrel — never from internal feature files directly.

```
// features/tasks/index.ts
export { TaskList }    from "./components/TaskList";
export { TaskCard }    from "./components/TaskCard";
export { useGetTasks } from "./hooks/useGetTasks";
export type { TaskViewModel, TaskCreateModel } from "./types/task.types";
```

```
// CORRECT — import from the feature barrel
import { TaskList } from "@/features/tasks";

// INCORRECT — import from internal file directly
import { TaskList } from "@/features/tasks/components/TaskList";
```

---

## Vertical Alignment Code Writing

Code follows a deliberate vertical rhythm. Each logical unit occupies its own line. Statements are never compressed to save space.

### One Statement Per Line

```
// CORRECT
const { data, isLoading, isError } = useGetTasks(filter);

if (isLoading)
    return <Spinner />;

if (isError)
    return <ErrorMessage />;

return <TaskList tasks={data} />;

// INCORRECT
const { data, isLoading, isError } = useGetTasks(filter); if (isLoading) return <Spinner />;
```

### Props — One Per Line (when more than two)

When a component has more than two props, each prop is placed on its own line.

```
// CORRECT
<TaskCard
    task={task}
    onDelete={handleDelete}
    onComplete={handleComplete}
/>

// INCORRECT
<TaskCard task={task} onDelete={handleDelete} onComplete={handleComplete} />
```

### Blank Lines Between Logical Blocks

A single blank line separates distinct logical blocks within a function body (guards, query call, render).

```
export function TaskList({ filter }: TaskListProps) {

    const { data, isLoading, isError } = useGetTasks(filter);

    if (isLoading)
        return <Spinner />;

    if (isError)
        return <ErrorMessage />;

    return (
        <ul>
            {data.map(task => (
                <TaskCard key={task.taskId} task={task} />
            ))}
        </ul>
    );
}
```

---

## TypeScript Rules

| Rule | Requirement |
|---|---|
| Strict mode | `strict: true` in `tsconfig.json` — always enabled |
| `any` type | Forbidden. Use `unknown` if the type is genuinely unknown |
| Non-null assertion (`!`) | Forbidden. Use optional chaining or explicit null checks |
| `type` vs `interface` | Use `type` for all definitions. `interface` is not used |
| Inline types | Forbidden for props — always define a named `Props` type |
| Implicit return types | Allowed for components. Required for all hook and API functions |

---

## Component Design

### One Component Per File

Each file exports exactly one component. Co-locating multiple components in one file is not allowed.

### Props Type Naming

```
// CORRECT
type TaskCardProps = {
    task:     TaskViewModel;
    onDelete: (taskId: string) => void;
};

export function TaskCard({ task, onDelete }: TaskCardProps) { ... }
```

### No Business Logic in Components

Components render data and fire events — they do not contain business logic, perform calculations, or call `infra/` directly.

---

## Unused Code

The codebase must contain zero unused artefacts.

| Category | Rule |
|---|---|
| `import` statements | Remove any import that is not referenced in the file |
| Variables & locals | Every declared variable must be read at least once after assignment |
| Props | Every defined prop must be used inside the component |
| Exported symbols | Every barrel export must be consumed from outside the feature |
| Type definitions | Every defined type must be used in at least one function or component |

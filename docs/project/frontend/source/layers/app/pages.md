# App — Pages

Pages are thin wrappers. They receive route params, pass them to feature components, and render layout. No business logic, no data transformation.

---

## Collection Page Pattern

```typescript
// app/tasks/page.tsx
import { TaskList } from "@/features/tasks";

export default function TasksPage() {
    return (
        <main>
            <h1>Tasks</h1>
            <TaskList filter={{}} />
        </main>
    );
}
```

---

## Detail Page Pattern

```typescript
// app/tasks/[taskId]/page.tsx
import { TaskDetail } from "@/features/tasks";

type TaskDetailPageProps = {
    params: { taskId: string };
};

export default function TaskDetailPage({ params }: TaskDetailPageProps) {
    return (
        <main>
            <TaskDetail taskId={params.taskId} />
        </main>
    );
}
```

---

## Rules

- Pages use `export default` — required by Next.js.
- No business logic, no data transformation, no direct API calls.
- Pages only pass route params down to feature components.

# Features — Types

All domain-specific TypeScript types live in `features/{domain}/types/{domain}.types.ts`.

---

## Pattern

```typescript
// features/tasks/types/task.types.ts

export type TaskStatus = "Pending" | "Completed";

export type TaskViewModel = {
    taskId:      string;
    title:       string;
    description: string | null;
    status:      TaskStatus;
    dueDate:     string | null;
    tags:        TagViewModel[];
    createdUtc:  string;
    updatedUtc:  string;
};

export type TaskCreateModel = {
    title:       string;
    description: string | null;
    dueDate:     string | null;
    tagIds:      string[];
};

export type TaskUpdateModel = {
    title:       string | null;
    description: string | null;
    dueDate:     string | null;
};

export type TaskFilterParams = {
    keyword?:     string;
    status?:      TaskStatus;
    tagId?:       string;
    dueDateFrom?: string;
    dueDateTo?:   string;
    sort?:        string;
    skip?:        number;
    take?:        number;
};
```

---

## Rules

- All types for one domain live in a single `{domain}.types.ts` file.
- Domain types never live in `core/types/` or `infra/` — they belong here.
- Types that must be shared across three or more unrelated features are moved to `core/types/`.

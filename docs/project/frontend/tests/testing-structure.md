# Testing — Structure

Tests are **co-located** alongside their source files. Each test file sits next to the file it tests.

---

## Folder Layout

```
features/
└── tasks/
    ├── hooks/
    │   ├── useGetTasks.ts
    │   ├── useGetTasks.test.ts      ← query hook test
    │   ├── useCreateTask.ts
    │   └── useCreateTask.test.ts    ← mutation hook test
    └── components/
        ├── TaskList.tsx
        ├── TaskList.test.tsx        ← component test
        ├── TaskCard.tsx
        └── TaskCard.test.tsx

core/
└── utils/
    ├── date.utils.ts
    └── date.utils.test.ts           ← utility test
```

---

## File Naming

| Source file | Test file |
|---|---|
| `useGetTasks.ts` | `useGetTasks.test.ts` |
| `useCreateTask.ts` | `useCreateTask.test.ts` |
| `TaskList.tsx` | `TaskList.test.tsx` |
| `date.utils.ts` | `date.utils.test.ts` |

One test file per source file. No shared or combined test files.

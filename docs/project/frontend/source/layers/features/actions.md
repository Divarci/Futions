# Features — Server Actions

When a domain operation requires a Server Action (e.g., a progressive-enhancement form), add an `actions/` folder to the domain. The `actions/` folder is optional — add it only when a Server Action is actually needed.

---

## Structure

```
features/{domain}/actions/{domain}.actions.ts
```

Server Actions are the frontend's command handlers — the direct analogy of the backend's Application layer Use Cases. Each action performs one operation and revalidates the affected path.

---

## Pattern

```typescript
// features/{domain}/actions/{domain}.actions.ts
"use server";

import { revalidatePath } from "next/cache";

export async function create{Entity}Action(formData: FormData): Promise<void> {
    const name = formData.get("name") as string;
    // call backend via httpClient or service
    revalidatePath("/{domain}");
}
```

---

## Rules

- Server Actions live in `features/{domain}/actions/{domain}.actions.ts` — not in `infra/`.
- Each action performs exactly one operation.
- Export all actions from the feature barrel (`index.ts`).
- Do **not** mark a function `"use server"` inside a component file — always isolate in `actions/`.

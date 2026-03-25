# Features — Hooks

SWR hooks that wrap `infra/` API call functions. Each hook wraps exactly one function.

---

## Query Hook — Collection

```typescript
// features/{domain}/hooks/useGet{Entities}.ts
import useSWR from "swr";
import { get{Entities} } from "@/infra/{domain}";
import type { {Entity}FilterParams } from "../types/{domain}.types";

export function useGet{Entities}(filter: {Entity}FilterParams) {
    return useSWR(["{domain}", filter], () => get{Entities}(filter));
}
```

---

## Query Hook — Single

```typescript
// features/{domain}/hooks/useGet{Entity}.ts
import useSWR from "swr";
import { get{Entity} } from "@/infra/{domain}";

export function useGet{Entity}({entity}Id: string) {
    return useSWR(
        {entity}Id ? ["{domain}", {entity}Id] : null,
        () => get{Entity}({entity}Id),
    );
}
```

---

## Mutation Hook

```typescript
// features/{domain}/hooks/useCreate{Entity}.ts
import useSWRMutation from "swr/mutation";
import { useSWRConfig } from "swr";
import { create{Entity} }   from "@/infra/{domain}";
import type { {Entity}CreateModel } from "../types/{domain}.types";

export function useCreate{Entity}() {

    const { mutate } = useSWRConfig();

    return useSWRMutation(
        ["{domain}"],
        (_key, { arg }: { arg: {Entity}CreateModel }) => create{Entity}(arg),
        { onSuccess: () => mutate((key) => Array.isArray(key) && key[0] === "{domain}", undefined, { revalidate: true }) },
    );
}
```

---

## Rules

- Each hook wraps exactly one `infra/` function — no combined calls.
- Hook names follow the pattern `use{Action}{Entity}` (e.g., `useGet{Entities}`, `useCreate{Entity}`).
- SWR keys are arrays: `["{domain}", params]`. Pass `null` as key to conditionally skip fetching.
- Mutation hooks revalidate related queries in `onSuccess` via the global `mutate`.
- Hooks must never call `infra/` functions that belong to a different domain.

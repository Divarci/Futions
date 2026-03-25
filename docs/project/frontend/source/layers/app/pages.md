# App — Pages

Pages are thin wrappers. They receive route params, pass them to feature components, and render layout. No business logic, no data transformation.

---

## Collection Page Pattern

```typescript
// app/{domain}/page.tsx
import { {Entity}List } from "@/features/{domain}";

export default function {Entities}Page() {
    return (
        <main>
            <h1>{Entities}</h1>
            <{Entity}List filter={{}} />
        </main>
    );
}
```

---

## Detail Page Pattern

```typescript
// app/{domain}/[{entity}Id]/page.tsx
import { {Entity}Detail } from "@/features/{domain}";

type {Entity}DetailPageProps = {
    params: { {entity}Id: string };
};

export default function {Entity}DetailPage({ params }: {Entity}DetailPageProps) {
    return (
        <main>
            <{Entity}Detail {entity}Id={params.{entity}Id} />
        </main>
    );
}
```

---

## Rules

- Pages use `export default` — required by Next.js.
- No business logic, no data transformation, no direct API calls.
- Pages only pass route params down to feature components.

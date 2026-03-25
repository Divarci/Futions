# Coding Style

This document establishes the code quality and consistency standards used across the frontend. All rules apply to every layer.

---

## Naming Conventions

### Files

| Element | Convention | Example |
|---|---|---|
| Component file | PascalCase | `{Entity}Card.tsx`, `{Entity}List.tsx` |
| Hook file | camelCase, `use` prefix | `useGet{Entities}.ts`, `useCreate{Entity}.ts` |
| API file | camelCase, `.api.ts` suffix | `{domain}.api.ts`, `common.types.ts` |
| Type file | camelCase, `.types.ts` suffix | `{domain}.types.ts`, `common.types.ts` |
| Utility file | camelCase, `.utils.ts` suffix | `date.utils.ts` |
| Constant file | camelCase, `.constants.ts` suffix | `api-routes.constants.ts` |
| Index/barrel file | `index.ts` | `features/{domain}/index.ts` |

### TypeScript Identifiers

| Element | Convention | Example |
|---|---|---|
| Component (function) | PascalCase | `{Entity}Card`, `{Entity}List` |
| Hook (function) | camelCase, `use` prefix | `useGet{Entities}`, `useCreate{Entity}` |
| Type | PascalCase | `{Entity}ViewModel`, `{Entity}CreateModel` |
| Enum | PascalCase | `{Entity}Status` |
| Enum value | PascalCase | `{Entity}Status.Active`, `{Entity}Status.Inactive` |
| Variable / parameter | camelCase | `{entity}Id`, `createModel` |
| Constant (module-level) | SCREAMING_SNAKE_CASE | `API_BASE_URL` |
| Props type | PascalCase, `Props` suffix | `{Entity}CardProps`, `{Entity}ListProps` |

### Async Functions

All async functions in hooks and API files are named to reflect their operation:

```
// hooks
useGet{Entities}(filter: {Entity}FilterParams)
useGet{Entity}({entity}Id: string)
useCreate{Entity}()
useUpdate{Entity}()
useDelete{Entity}()

// api functions
get{Entities}(filter: {Entity}FilterParams): Promise<{Entity}ViewModel[]>
get{Entity}({entity}Id: string): Promise<{Entity}ViewModel>
create{Entity}(model: {Entity}CreateModel): Promise<{Entity}ViewModel>
update{Entity}({entity}Id: string, model: {Entity}UpdateModel): Promise<{Entity}ViewModel>
delete{Entity}({entity}Id: string): Promise<void>
```

---

## Exports

### Named Exports Only

All files use named exports. Default exports are only used where Next.js requires them (`page.tsx`, `layout.tsx`, `error.tsx`, `loading.tsx`).

```
// CORRECT
export function {Entity}Card({ {entity} }: {Entity}CardProps) { ... }
export type { {Entity}ViewModel };

// INCORRECT — no default exports in components or hooks
export default function {Entity}Card() { ... }
```

### Barrel Files (`index.ts`)

Each feature folder exposes a public API through its `index.ts`. Files outside the feature import only from the barrel — never from internal feature files directly.

```
// features/{domain}/index.ts
export { {Entity}List }     from "./components/{Entity}List";
export { {Entity}Card }     from "./components/{Entity}Card";
export { useGet{Entities} } from "./hooks/useGet{Entities}";
export type { {Entity}ViewModel, {Entity}CreateModel } from "./types/{domain}.types";
```

```
// CORRECT — import from the feature barrel
import { {Entity}List } from "@/features/{domain}";

// INCORRECT — import from internal file directly
import { {Entity}List } from "@/features/{domain}/components/{Entity}List";
```

---

## Vertical Alignment Code Writing

Code follows a deliberate vertical rhythm. Each logical unit occupies its own line. Statements are never compressed to save space.

### One Statement Per Line

```
// CORRECT
const { data, isLoading, isError } = useGet{Entities}(filter);

if (isLoading)
    return <Spinner />;

if (isError)
    return <ErrorMessage />;

return <{Entity}List {entities}={data} />;

// INCORRECT
const { data, isLoading, isError } = useGet{Entities}(filter); if (isLoading) return <Spinner />;
```

### Props — One Per Line (when more than two)

When a component has more than two props, each prop is placed on its own line.

```
// CORRECT
<{Entity}Card
    {entity}={{entity}}
    onDelete={handleDelete}
    onComplete={handleComplete}
/>

// INCORRECT
<{Entity}Card {entity}={{entity}} onDelete={handleDelete} onComplete={handleComplete} />
```

### Blank Lines Between Logical Blocks

A single blank line separates distinct logical blocks within a function body (guards, query call, render).

```
export function {Entity}List({ filter }: {Entity}ListProps) {

    const { data, isLoading, isError } = useGet{Entities}(filter);

    if (isLoading)
        return <Spinner />;

    if (isError)
        return <ErrorMessage />;

    return (
        <ul>
            {data.map({entity} => (
                <{Entity}Card key={{entity}.{entity}Id} {entity}={{entity}} />
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
type {Entity}CardProps = {
    {entity}:   {Entity}ViewModel;
    onDelete: ({entity}Id: string) => void;
};

export function {Entity}Card({ {entity}, onDelete }: {Entity}CardProps) { ... }
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

---

## Responsive Design

All components, feature UIs, and pages must be fully responsive. Use **mobile-first** design: base styles target the smallest screen; breakpoint prefixes progressively enhance for larger screens.

### Breakpoints (Tailwind defaults)

| Prefix | Min-width | Target |
|---|---|---|
| _(none)_ | 0 px | Mobile (base) |
| `sm:` | 640 px | Large mobile / small tablet |
| `md:` | 768 px | Tablet |
| `lg:` | 1024 px | Desktop |
| `xl:` | 1280 px | Wide desktop |

### Rules

- Write base styles for mobile first — never use `max-width` overrides to target small screens.
- Multi-column grid and flex layouts must collapse to single column on mobile.
- Never hardcode a fixed pixel width (`w-[600px]`) without a responsive counterpart.
- Font sizes, padding, and spacing must scale appropriately across breakpoints.

### Pattern

```tsx
// CORRECT — mobile-first, progressively enhanced
<div className="grid grid-cols-1 gap-4 md:grid-cols-2 lg:grid-cols-3">
    {items.map(item => <Card key={item.id} item={item} />)}
</div>

// INCORRECT — desktop assumption, no mobile consideration
<div className="grid grid-cols-3 gap-4">
    {items.map(item => <Card key={item.id} item={item} />)}
</div>
```

---

## Theme

All components must be **theme-aware**: use semantic token classes instead of raw Tailwind colour utilities so the UI responds correctly to the active theme (light / dark / system).

See the full token contract, CSS variable definitions, and Tailwind config extension in [`core/theme.md`](../layers/core/theme.md).

### Rules

- Use token classes: `bg-background`, `text-foreground`, `bg-primary`, `text-primary-fg`, `bg-muted`, `text-muted-fg`, `border-border`, `bg-destructive`, `text-destructive-fg`.
- Never use raw colour utilities on semantic surfaces: `bg-white`, `bg-gray-*`, `text-black`, `text-gray-*` are forbidden.
- Raw colour utilities are allowed only for non-semantic decoration (chart fills, illustration tints).

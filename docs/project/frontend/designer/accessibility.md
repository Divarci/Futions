# Designer — Accessibility

Accessibility is not optional. Every design must meet WCAG AA standards out of the box.

---

## Color Contrast Requirements

| Text type | Minimum ratio |
|---|---|
| Normal text (< 18px / < 14px bold) | 4.5 : 1 |
| Large text (≥ 18px / ≥ 14px bold) | 3 : 1 |
| UI components (borders, icons) | 3 : 1 |

**Verifying token pairs:**

After adjusting token values for a new design, verify the following pairs pass contrast requirements:

| Background token | Foreground token | Minimum |
|---|---|---|
| `bg-background` | `text-foreground` | 7:1 (target AA+) |
| `bg-background` | `text-muted-fg` | 4.5:1 |
| `bg-primary` | `text-primary-fg` | 4.5:1 |
| `bg-muted` | `text-foreground` | 4.5:1 |
| `bg-destructive` | `text-destructive-fg` | 4.5:1 |

Use [WebAIM Contrast Checker](https://webaim.org/resources/contrastchecker/) or Figma's built-in contrast plugin to verify before finalizing token values.

---

## Focus Management

All interactive elements must have visible focus styles for keyboard navigation. Use `:focus-visible` (not `:focus`) so focus rings only appear during keyboard navigation, not mouse click.

```css
/* globals.css — global focus style */
:focus-visible {
    outline: 2px solid var(--color-primary);
    outline-offset: 2px;
    border-radius: 4px;
}
```

Tailwind equivalent on interactive elements:

```tsx
className="focus-visible:ring-2 focus-visible:ring-primary focus-visible:outline-none rounded-md"
```

---

## ARIA Labels

### Icon-only buttons

```tsx
<button aria-label="Open notifications" className="...">
    <BellIcon aria-hidden="true" />
</button>
```

### Form fields

```tsx
<label htmlFor="email" className="text-sm font-medium text-foreground">
    Email <span className="text-destructive">*</span>
</label>
<input
    id="email"
    type="email"
    aria-describedby="email-hint email-error"
    aria-required="true"
    className="..."
/>
<p id="email-hint" className="text-sm text-muted-fg">Use your work email</p>
<p id="email-error" role="alert" className="text-sm text-destructive-fg">
    Please enter a valid email
</p>
```

### Loading states

```tsx
<button aria-busy="true" aria-label="Saving..." className="..." disabled>
    <Spinner aria-hidden="true" /> Saving...
</button>
```

### Modal dialogs

```tsx
<div
    role="dialog"
    aria-modal="true"
    aria-labelledby="modal-title"
    aria-describedby="modal-description"
>
    <h2 id="modal-title">Confirm deletion</h2>
    <p id="modal-description">This action cannot be undone.</p>
</div>
```

---

## Keyboard Navigation

| Element | Required behavior |
|---|---|
| All buttons and links | Reachable via `Tab`; activated via `Enter` or `Space` |
| Modals | Focus trapped inside while open; `Esc` closes |
| Dropdowns / menus | Arrow keys navigate items; `Esc` closes |
| Form fields | `Tab` order follows visual reading order |
| Icon-only actions | `aria-label` always present |

---

## Touch Target Size

On mobile, all interactive elements must have a minimum **44×44px** touch target.

```tsx
// Use padding to expand small icons to meet the 44px target
<button className="p-3 rounded-md">  {/* 24px icon + 12px padding = 48px */}
    <XIcon className="w-6 h-6" />
</button>
```

---

## Pre-Delivery Accessibility Checklist

```
□ --color-foreground vs --color-background contrast ≥ 7:1
□ --color-muted-fg vs --color-background contrast ≥ 4.5:1
□ --color-primary-fg vs --color-primary contrast ≥ 4.5:1
□ All other extended token pairs verified against contrast requirements
□ Focus-visible styles present on all interactive elements
□ All icon-only buttons have aria-label
□ All form inputs have associated <label> and aria-describedby
□ Modals have role="dialog", aria-modal, aria-labelledby and focus-trap
□ Mobile touch targets ≥ 44×44px
□ No information conveyed by color alone
```

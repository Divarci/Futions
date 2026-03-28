# Designer — Component Library

Rules for designing every category of UI component. All components must use semantic token classes and be mobile-first responsive.

---

## Component Anatomy

Every component is composed of these layers:

```
Component
├── Base Layer        → Core HTML structure
├── Style Layer       → Token class bindings (bg-background, text-foreground, border-border, etc.)
├── State Layer       → hover / focus / active / disabled / loading
├── Variant Layer     → size / intent / appearance variants
└── Composition Layer → Rules for nesting with other components
```

---

## Button System

```tsx
type ButtonVariant = 'primary' | 'secondary' | 'ghost' | 'danger' | 'link'
type ButtonSize    = 'xs' | 'sm' | 'md' | 'lg' | 'xl'

// Structural rules:
// - Min width: 80px
// - Horizontal padding ≥ 2× vertical padding
// - Icon + label gap: gap-2 (8px)
// - Loading state: Spinner + label or skeleton pulse
// - Disabled: opacity-50, cursor-not-allowed, pointer-events-none
```

**Variant token classes:**

```tsx
// Primary — main call to action (max 1 per page)
<button className="bg-primary text-primary-fg hover:opacity-90 rounded-md px-4 py-2 min-w-[80px]">
    Save
</button>

// Secondary — supporting actions
<button className="border border-border bg-background text-foreground hover:bg-muted rounded-md px-4 py-2">
    Cancel
</button>

// Ghost — navigation, low-emphasis
<button className="bg-transparent text-foreground hover:bg-muted rounded-md px-4 py-2">
    View
</button>

// Danger — irreversible actions only
<button className="bg-destructive text-destructive-fg hover:opacity-90 rounded-md px-4 py-2">
    Delete
</button>
```

**Button Hierarchy:**
- Maximum **1 primary** button per page
- Secondary buttons for supporting actions
- Ghost buttons for menus, navigation, low-emphasis actions
- Danger only for irreversible actions (delete, revoke)

---

## Form Elements

```
Input States (token classes):
  Default  → border border-border bg-background text-foreground
  Hover    → border-muted-fg
  Focus    → border-primary ring-2 ring-primary/20 outline-none
  Error    → border-destructive; error message below in text-sm text-destructive-fg
  Disabled → bg-muted text-muted-fg opacity-60 cursor-not-allowed
  Success  → border-success (extended token); checkmark icon trailing

Label position:
  - Always above the input
  - Required marker: asterisk (*) in text-destructive next to label text
  - Helper text: below input, text-sm text-muted-fg

Validation:
  - Triggers onBlur, not onChange
  - Messages: specific and actionable ("At least 8 characters" ✓ / "Invalid" ✗)
```

---

## Card Component

```tsx
// flat — no border, muted background
<div className="bg-muted rounded-lg p-4 md:p-6">...</div>

// outlined — border visible, page-background fill
<div className="border border-border bg-background rounded-lg p-4 md:p-6">...</div>

// elevated — shadow, slightly raised
<div className="bg-muted shadow-md rounded-lg p-4 md:p-6">...</div>

// interactive — elevated + hover lift animation
<div className="bg-muted shadow-md rounded-lg p-4 md:p-6 transition-all hover:-translate-y-0.5 hover:shadow-lg cursor-pointer">
    ...
</div>
```

**Card content hierarchy:**
1. Media (optional) — `aspect-video` or `aspect-[4/3]`, `w-full object-cover`
2. Category / Badge
3. Title — `line-clamp-2 font-semibold text-foreground`
4. Description — `line-clamp-3 text-muted-fg text-sm`
5. Meta (date, author) — `text-xs text-muted-fg`
6. Action area — `Button` or `Link`

---

## Navigation

```
Nav Types:
  Sidebar    → Dashboard layouts; 240–280px fixed width
  Topbar     → Content sites; sticky
  Bottom Tab → Mobile-first experiences

Active State:
  Sidebar    → bg-muted text-foreground + left-side border border-primary
  Topbar     → text-primary + border-b border-primary
  Bottom Tab → text-primary icon + label becomes visible

Mobile Behavior:
  Sidebar → hidden on mobile; open as drawer overlay (z-50)
  Topbar  → hamburger icon → full-screen menu overlay
```

```tsx
// Sidebar: desktop visible, mobile hidden
<aside className="hidden md:flex w-60 shrink-0 flex-col bg-background border-r border-border">

// Mobile sidebar drawer
<div className="fixed inset-0 z-50 flex md:hidden">
    <div className="w-60 bg-background border-r border-border">...</div>
    <div className="flex-1 bg-foreground/50" onClick={close} />
</div>
```

---

## Modal & Overlay

```
Size Scale:
  xs:   max-w-sm  (384px) → Confirmation dialog
  sm:   max-w-md  (448px) → Simple form
  md:   max-w-lg  (512px) → Medium form
  lg:   max-w-2xl (672px) → Complex content
  xl:   max-w-4xl (896px) → Dashboard panel
  full: 95vw              → Full-screen editor

Container token classes:
  bg-background border border-border shadow-xl rounded-xl

Animation:
  Enter: scale(0.95) opacity(0) → scale(1) opacity(1), 200ms ease-out
  Exit:  scale(1) opacity(1) → scale(0.95) opacity(0), 150ms ease-in
  Backdrop: bg-foreground/50, fade in 200ms

Accessibility (required):
  - focus-trap active while open
  - ESC closes the modal
  - aria-modal="true" on the dialog element
  - aria-labelledby pointing to the modal title
```

---

## Loading & Empty States

Every data-fetching component must have all three states designed before it is considered complete.

**Loading state:** Centered spinner with sufficient vertical space. Use the `Spinner` primitive.

**Error state:** Clear, non-technical message. Use the `ErrorMessage` primitive. Message explains what failed and optionally what to do next.

**Empty state:**
```tsx
<div className="flex flex-col items-center justify-center py-16 text-center">
    <p className="text-lg font-semibold text-foreground">No items yet</p>
    <p className="mt-1 text-sm text-muted-fg">Create your first item to get started.</p>
</div>
```
- Heading: `text-lg font-semibold text-foreground`
- Supporting text: `text-sm text-muted-fg`
- Optional action button below (primary variant)

---

## Pre-Delivery Component Checklist

```
□ All component colors use token classes — no raw utilities on semantic surfaces
□ All interactive states designed: hover, focus, active, disabled, loading
□ Mobile-first: base styles are for mobile, md:/lg: enhance larger screens
□ Loading, error, and empty states all present for data-fetching components
□ Button hierarchy respected — max 1 primary per page
□ Modals have focus-trap, ESC handler, and correct ARIA attributes
□ Cards use consistent radius and padding scale
```

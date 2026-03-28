# Designer — Theme System

The project uses a **semantic token layer** on top of Tailwind CSS, powered by `next-themes` and CSS custom properties. Every design output must comply with this system — there are no exceptions.

---

## Non-Negotiable Baseline Rules

1. **Every component uses token classes** — `bg-background`, `text-foreground`, `bg-muted`, `border-border`, etc.
2. **Raw color utilities are forbidden on semantic surfaces** — `bg-white`, `bg-gray-100`, `text-black`, `text-gray-700` must not appear on any meaningful UI surface.
3. **Every new design adjusts the token values** — the defaults in `globals.css` are a generic baseline, not a final palette. See the Token Adjustment section below.
4. **Both light and dark values must always be set** — a half-adjusted token contract is not acceptable.

---

## Token Contract

Tokens are CSS custom properties defined in `app/(protected)/globals.css`:

```css
/* globals.css */
:root {
    --color-background:     #ffffff;
    --color-foreground:     #0a0a0a;
    --color-primary:        #1d4ed8;
    --color-primary-fg:     #ffffff;
    --color-muted:          #f3f4f6;
    --color-muted-fg:       #6b7280;
    --color-border:         #e5e7eb;
    --color-destructive:    #dc2626;
    --color-destructive-fg: #ffffff;
}

.dark {
    --color-background:     #0a0a0a;
    --color-foreground:     #fafafa;
    --color-primary:        #3b82f6;
    --color-primary-fg:     #ffffff;
    --color-muted:          #1c1c1e;
    --color-muted-fg:       #a1a1aa;
    --color-border:         #27272a;
    --color-destructive:    #ef4444;
    --color-destructive-fg: #ffffff;
}
```

Tokens are bridged into Tailwind in `tailwind.config.ts` so they are usable as utility classes:

```ts
theme: {
    extend: {
        colors: {
            background:       "var(--color-background)",
            foreground:       "var(--color-foreground)",
            primary:          "var(--color-primary)",
            "primary-fg":     "var(--color-primary-fg)",
            muted:            "var(--color-muted)",
            "muted-fg":       "var(--color-muted-fg)",
            border:           "var(--color-border)",
            destructive:      "var(--color-destructive)",
            "destructive-fg": "var(--color-destructive-fg)",
        }
    }
}
```

---

## Token Usage Reference

| Token class | Use for |
|---|---|
| `bg-background` | Page background |
| `text-foreground` | Primary body text |
| `bg-primary` | Primary action surfaces (buttons, highlights) |
| `text-primary-fg` | Text on top of primary surfaces |
| `bg-muted` | Card backgrounds, subtle containers |
| `text-muted-fg` | Secondary / supporting text, placeholders |
| `border-border` | All borders and dividers |
| `bg-destructive` | Destructive action surfaces |
| `text-destructive-fg` | Text on top of destructive surfaces |

Raw color utilities are acceptable **only for non-semantic decoration** — chart fills, illustration tints, data visualizations.

---

## Full Token Adjustment — Required for Every New Design

> **The default token values are a generic starting point. Every new design begins by re-deriving all tokens from the prompt.**

When a design prompt is received, all of the following tokens must be evaluated and adjusted for both `:root` (light) and `.dark`:

| Token | What it represents | Driven by |
|---|---|---|
| `--color-background` | Page surface | Overall warmth/coolness of the palette |
| `--color-foreground` | Primary text | Must contrast ≥ 7:1 against `background` for body text |
| `--color-primary` | Brand / action color | Core brand emotion extracted from prompt |
| `--color-primary-fg` | Text on primary | Must contrast ≥ 4.5:1 against `primary` |
| `--color-muted` | Subtle containers | Desaturated / tinted variant of `background` |
| `--color-muted-fg` | Supporting text | Mid-contrast against `background`, clearly readable |
| `--color-border` | Dividers and outlines | Subtle - visible but not distracting |
| `--color-destructive` | Error / danger | Usually stays near red unless brand has a strong reason |
| `--color-destructive-fg` | Text on destructive | Must contrast ≥ 4.5:1 against `destructive` |

**Derivation Protocol:**

1. Extract `--color-primary` from the dominant brand emotion (see `prompt-analysis.md`)
2. Set light `--color-background` based on warmth axis (warm whites: `#faf9f7`, cool whites: `#f8fafc`, neutral: `#ffffff`)
3. Set dark `--color-background` to complement the brand (warm brand → warm dark `#0f0e0d`, cool brand → cool dark `#0a0a0f`)
4. Derive `--color-muted` as a tinted, desaturated step above `background`
5. Set `--color-border` as a subtle step between `background` and `muted`
6. Verify all contrast ratios before finalizing

**Example — warm, friendly SaaS prompt:**
```css
:root {
    --color-background:     #faf9f7;   /* warm off-white */
    --color-foreground:     #1a1714;   /* warm near-black */
    --color-primary:        #d97706;   /* amber — warm action color */
    --color-primary-fg:     #ffffff;
    --color-muted:          #f0ede8;   /* warm light container */
    --color-muted-fg:       #78716c;   /* warm mid-tone */
    --color-border:         #e7e3dd;   /* warm subtle border */
    --color-destructive:    #dc2626;
    --color-destructive-fg: #ffffff;
}
.dark {
    --color-background:     #110f0d;   /* warm near-black */
    --color-foreground:     #faf9f7;
    --color-primary:        #f59e0b;   /* slightly lighter amber in dark */
    --color-primary-fg:     #1a1714;
    --color-muted:          #1e1b17;
    --color-muted-fg:       #a8a29e;
    --color-border:         #2c2824;
    --color-destructive:    #ef4444;
    --color-destructive-fg: #ffffff;
}
```

---

## How Dark Mode Works

Dark mode is class-driven. `next-themes` adds the `.dark` class to `<html>` when dark mode is active. Because both `:root` and `.dark` define all tokens, every component using token classes automatically adapts — no `dark:` inline variants are needed.

```tsx
// Mounted once in the root layout
<ThemeProvider attribute="class" defaultTheme="system" enableSystem>
    {children}
</ThemeProvider>
```

A `ThemeToggle` primitive in `core/components/ThemeToggle/` provides the switch UI using `useTheme()` from `next-themes`.

---

## Token Extension Protocol

When a design requires semantic colors beyond the base contract (e.g., success states, warning indicators), extend the contract — never use raw utilities as a workaround.

Add new tokens to **both** `:root` and `.dark` in `globals.css`, then register them in `tailwind.config.ts`:

```css
/* globals.css extension example */
:root {
    --color-success:    #22c55e;
    --color-success-fg: #ffffff;
    --color-warning:    #f59e0b;
    --color-warning-fg: #ffffff;
    --color-info:       #3b82f6;
    --color-info-fg:    #ffffff;
}
.dark {
    --color-success:    #16a34a;
    --color-success-fg: #ffffff;
    --color-warning:    #d97706;
    --color-warning-fg: #ffffff;
    --color-info:       #2563eb;
    --color-info-fg:    #ffffff;
}
```

---

## Pre-Delivery Theme Checklist

```
□ --color-primary and --color-primary-fg updated for both :root and .dark
□ --color-background adjusted to match the design's warmth/coolness for both :root and .dark
□ --color-foreground verified to contrast ≥ 7:1 against background in both modes
□ --color-muted and --color-muted-fg feel cohesive with the primary palette
□ --color-border is visible but subtle in both modes
□ All extended tokens (success, warning, info) defined in both :root and .dark
□ Tailwind config updated for any extended tokens
□ No bg-white, bg-gray-*, text-black, text-gray-* on semantic surfaces
□ No dark: inline variants used — all switching handled by token reassignment
```

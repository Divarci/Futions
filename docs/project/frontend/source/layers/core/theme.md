# Core — Theme

The theme system defines a semantic design-token layer on top of Tailwind CSS. **Every component must use token-based classes — not raw colour utilities.** This guarantees all UI responds correctly when the active theme changes.

---

## Stack

| Technology | Role |
|---|---|
| `next-themes` | `ThemeProvider` wrapper and `useTheme` hook |
| Tailwind CSS (`darkMode: "class"`) | Class-driven dark/light variant switching |
| CSS custom properties | Token values defined per-theme in `globals.css` |

---

## Token Contract

Tokens are CSS custom properties defined in `globals.css`:

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

Tokens are extended in `tailwind.config.ts` so they are available as Tailwind utilities:

```ts
// tailwind.config.ts
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

## Usage Rules

- **Always use token classes**: `bg-background`, `text-foreground`, `bg-primary`, `text-primary-fg`, `bg-muted`, `text-muted-fg`, `border-border`, `bg-destructive`, `text-destructive-fg`.
- **Never use raw colour utilities** on semantic surfaces: `bg-white`, `bg-gray-100`, `text-black`, `text-gray-700` are forbidden.
- Raw colour utilities are allowed only for non-semantic decoration (e.g., a chart fill, illustration tint).

---

## ThemeProvider Setup

`ThemeProvider` is mounted once in `app/layout.tsx`. `attribute="class"` adds `.dark` to `<html>` when dark mode is active.

```tsx
// app/layout.tsx
import { ThemeProvider } from "next-themes";

export default function RootLayout({ children }: { children: React.ReactNode }) {
    return (
        <html lang="en" suppressHydrationWarning>
            <body className="bg-background text-foreground">
                <ThemeProvider
                    attribute="class"
                    defaultTheme="system"
                    enableSystem
                >
                    {children}
                </ThemeProvider>
            </body>
        </html>
    );
}
```

---

## ThemeToggle Component

A `ThemeToggle` primitive in `core/components/ThemeToggle/` provides the theme switch UI.

```tsx
// core/components/ThemeToggle/ThemeToggle.tsx
"use client";

import { useTheme } from "next-themes";

export function ThemeToggle() {

    const { theme, setTheme } = useTheme();

    return (
        <button
            className="rounded-md border border-border bg-muted p-2 text-muted-fg hover:bg-primary hover:text-primary-fg"
            onClick={() => setTheme(theme === "dark" ? "light" : "dark")}
        >
            {theme === "dark" ? "Light" : "Dark"}
        </button>
    );
}
```

---

## Theme Type

`Theme` is defined in `core/types/theme.types.ts`:

```ts
export type Theme = "light" | "dark" | "system";
```

---

## Non-Negotiable Checklist

Before completing any component or page, verify:

- [ ] All background colours use a token class (`bg-background`, `bg-muted`, `bg-primary`)
- [ ] All text colours use a token class (`text-foreground`, `text-muted-fg`, `text-primary-fg`)
- [ ] All border colours use a token class (`border-border`)
- [ ] No `bg-white`, `bg-gray-*`, `text-black`, `text-gray-*` on semantic surfaces

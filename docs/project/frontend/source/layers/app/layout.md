# App — Root Layout

The root layout provides global context providers (theme) and wraps the entire protected application. It lives inside the `(protected)/` route group.

---

## Pattern

```typescript
// app/(protected)/layout.tsx
import type { Metadata } from "next";
import type { ReactNode } from "react";
import { Providers } from "@/core/components";
import "./globals.css";

export const metadata: Metadata = {
    title: "Futions",
    description: "Futions application",
};

type RootLayoutProps = {
    children: ReactNode;
};

export default function RootLayout({ children }: RootLayoutProps) {
    return (
        <html lang="en" suppressHydrationWarning>
            <body className="bg-background text-foreground min-h-screen">
                <Providers>
                    {children}
                </Providers>
            </body>
        </html>
    );
}
```

---

## Rules

- The root layout lives at `app/(protected)/layout.tsx` — not at the `app/` root.
- The root layout wraps all protected routes in global providers.
- The `Providers` component lives in `core/components/` and encapsulates theme and other global context.
- Global styles (`globals.css`) are imported here — they also live in `(protected)/`.
- Uses `export default` — required by Next.js.
- `suppressHydrationWarning` on `<html>` is required for theme toggling to work without hydration mismatch.

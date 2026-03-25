# App — Root Layout

The root layout provides global context providers (SWR, theme) and wraps the entire application.

---

## Pattern

```typescript
// app/layout.tsx
import { Providers } from "@/core/components";
import type { ReactNode } from "react";

type RootLayoutProps = {
    children: ReactNode;
};

export default function RootLayout({ children }: RootLayoutProps) {
    return (
        <html lang="en">
            <body>
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

- The root layout wraps the entire application in global providers.
- The `Providers` component lives in `core/components/` and encapsulates `SWRConfig`, theme, and other global context.
- Uses `export default` — required by Next.js.

"use client";

import { ThemeProvider } from "next-themes";
import type { ProvidersProps } from "./Providers.types";

export function Providers({ children }: ProvidersProps) {

    return (
        <ThemeProvider
            attribute="class"
            defaultTheme="system"
            enableSystem
        >
            {children}
        </ThemeProvider>
    );
}

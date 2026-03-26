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

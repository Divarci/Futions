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

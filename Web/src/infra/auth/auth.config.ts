import KeycloakProvider from "next-auth/providers/keycloak";
import type { NextAuthOptions } from "next-auth";
import type { JWT } from "next-auth/jwt";
import "./auth.types";

// ---------------------------------------------------------------------------
// Token refresh
// ---------------------------------------------------------------------------

async function refreshAccessToken(token: JWT): Promise<JWT> {
    try {
        const response = await fetch(
            `${process.env.KEYCLOAK_ISSUER}/protocol/openid-connect/token`,
            {
                method:  "POST",
                headers: { "Content-Type": "application/x-www-form-urlencoded" },
                body:    new URLSearchParams({
                    grant_type:    "refresh_token",
                    client_id:     process.env.KEYCLOAK_CLIENT_ID!,
                    client_secret: process.env.KEYCLOAK_CLIENT_SECRET!,
                    refresh_token: token.refreshToken as string,
                }),
            }
        );

        const refreshed = await response.json();
        if (!response.ok) throw refreshed;

        return {
            ...token,
            accessToken:  refreshed.access_token               as string,
            refreshToken: (refreshed.refresh_token ?? token.refreshToken) as string,
            expiresAt:    Math.floor(Date.now() / 1000) + (refreshed.expires_in as number),
            error:        undefined,
        };
    } catch {
        return { ...token, error: "RefreshAccessTokenError" };
    }
}

// ---------------------------------------------------------------------------
// Auth options — swap the provider block when changing auth providers.
// Everything else (callbacks, module augmentation) remains unchanged.
// ---------------------------------------------------------------------------

export const authOptions: NextAuthOptions = {

    // ── Provider ──────────────────────────────────────────────────────────
    // See: docs/project/frontend/source/layers/app/auth-provider.md
    providers: [
        KeycloakProvider({
            clientId:     process.env.KEYCLOAK_CLIENT_ID!,
            clientSecret: process.env.KEYCLOAK_CLIENT_SECRET!,
            issuer:       process.env.KEYCLOAK_ISSUER!,
        }),
    ],

    // ── Callbacks ─────────────────────────────────────────────────────────
    callbacks: {

        async jwt({ token, account, profile }) {
            // Initial sign-in: persist tokens and user identity
            if (account) {
                token.accessToken  = account.access_token;
                token.refreshToken = account.refresh_token;
                token.expiresAt    = account.expires_at;
                token.user         = {
                    id:    (profile as { sub:   string }).sub,
                    email: (profile as { email: string }).email,
                };
            }

            // Token still valid
            if (token.expiresAt && Date.now() < token.expiresAt * 1000) {
                return token;
            }

            // No refresh token yet (shouldn't happen after sign-in, guards against edge cases)
            if (!token.refreshToken) {
                return token;
            }

            // Token expired — attempt refresh
            return refreshAccessToken(token);
        },

        async session({ session, token }) {
            session.accessToken = token.accessToken;
            session.error       = token.error;
            return session;
        },
    },
};

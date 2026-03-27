# Auth Provider

This document describes the configured authentication provider. The authentication **framework** is always NextAuth.js v4 — it does not change. Only the provider block changes when switching identity providers.

**Current provider: Keycloak**

---

## How Provider Swapping Works

Authentication is split into two concerns:

| Concern | Location | Changes on swap? |
|---|---|---|
| Auth framework | `next-auth` (NextAuth.js v4) | Never |
| Provider config | `src/infra/auth/auth.config.ts` — `providers` array | Yes — only this block |
| JWT/session callbacks | `src/infra/auth/auth.config.ts` — `callbacks` | Only if the new provider has a different token shape |
| Module augmentation | `src/infra/auth/auth.types.ts` | Rarely |
| Route handler | `src/app/(public)/api/auth/[...nextauth]/route.ts` | Never |
| Middleware | `src/middleware.ts` | Never |

**To switch providers:** update only the `providers` array inside `auth.config.ts`. If the new provider uses a different refresh token endpoint, also update `refreshAccessToken`. Nothing else changes.

---

## Current Provider — Keycloak

### Environment Variables

| Variable | Description | Example |
|---|---|---|
| `KEYCLOAK_CLIENT_ID` | OAuth2 client ID registered in Keycloak | `futions-web` |
| `KEYCLOAK_CLIENT_SECRET` | Client secret from Keycloak client credentials | — |
| `KEYCLOAK_ISSUER` | Realm issuer URL | `https://auth.example.com/realms/futions` |
| `NEXTAUTH_SECRET` | Random secret used to sign/encrypt JWTs | `openssl rand -base64 32` |
| `NEXTAUTH_URL` | Canonical base URL of the app | `https://app.example.com` |

### Keycloak Client Configuration

The Keycloak client should be configured with:
- **Access type:** confidential
- **Valid redirect URIs:** `{NEXTAUTH_URL}/api/auth/callback/keycloak`
- **Web origins:** `{NEXTAUTH_URL}`
- **Standard flow:** enabled
- **Direct access grants:** disabled

### Files

```
src/
├── infra/
│   └── auth/
│       ├── auth.config.ts   ← Keycloak provider + refresh token logic
│       ├── auth.types.ts    ← NextAuth module augmentation (Session, JWT)
│       └── index.ts
└── app/
    └── (public)/
        └── api/
            └── auth/
                └── [...nextauth]/
                    └── route.ts   ← NextAuth GET/POST handler
```

### Token Flow

1. User visits a protected route → middleware redirects to `/api/auth/signin`
2. NextAuth renders the Keycloak sign-in button → user is redirected to Keycloak
3. Keycloak authenticates and redirects back to `/api/auth/callback/keycloak`
4. NextAuth `jwt` callback stores `access_token`, `refresh_token`, and `expires_at` in the encrypted JWT cookie
5. On each request, middleware validates the JWT and injects `x-user-id` / `x-user-email` headers
6. When the access token expires, the `jwt` callback calls the Keycloak token endpoint to refresh it
7. If refresh fails, token gets `error: "RefreshAccessTokenError"` → middleware forces re-login

---

## Switching to a Different Provider

1. **Remove** `KeycloakProvider` import and configuration from `auth.config.ts`
2. **Add** the new provider import (e.g., `import GitHubProvider from "next-auth/providers/github"`)
3. **Replace** the entry in the `providers` array
4. **Update** `refreshAccessToken` if the new provider uses a different refresh token endpoint (or remove it if the provider handles refresh automatically)
5. **Update** environment variables in `.env.local` and deployment secrets
6. **Update** this file (`auth-provider.md`) to reflect the new provider

**Everything else stays the same.**

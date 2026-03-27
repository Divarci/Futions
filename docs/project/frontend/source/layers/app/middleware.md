# App — Middleware

`middleware.ts` is the global authentication guard. It runs on every request matched by the `config.matcher` and enforces JWT-based authentication before the request reaches any route handler or page.

---

## Location

```
src/
└── middleware.ts       ← must stay at the src/ root (Next.js convention)
```

---

## Behaviour

The middleware checks for a valid NextAuth JWT on every matched request.

| Condition | Action |
|---|---|
| No token present | Redirect to `/api/auth/signin?callbackUrl={original path}` |
| Token has `error: "RefreshAccessTokenError"` | Redirect to `/api/auth/signin?error=SessionExpired&callbackUrl={original path}` |
| Valid token | Allow request through; inject `x-user-id` and `x-user-email` headers |

---

## Pattern

```typescript
// src/middleware.ts
import { getToken } from "next-auth/jwt";
import type { NextRequest } from "next/server";
import { NextResponse } from "next/server";

export async function middleware(req: NextRequest) {
    const { pathname, search } = req.nextUrl;

    const token = await getToken({
        req,
        secret: process.env.NEXTAUTH_SECRET,
    });

    const fullPath = pathname + search;

    // No token → redirect to signin with full callbackUrl
    if (!token) {
        const url = req.nextUrl.clone();
        url.pathname = "/api/auth/signin";
        url.searchParams.set("callbackUrl", fullPath);
        return NextResponse.redirect(url);
    }

    // Token refresh error → force re-login
    if (token.error === "RefreshAccessTokenError") {
        const url = req.nextUrl.clone();
        url.pathname = "/api/auth/signin";
        url.searchParams.set("error", "SessionExpired");
        url.searchParams.set("callbackUrl", fullPath);
        return NextResponse.redirect(url);
    }

    // Pass user info via headers
    const response = NextResponse.next();

    if (token.user) {
        response.headers.set("x-user-id", (token.user as { id: string }).id);
        response.headers.set("x-user-email", (token.user as { email: string }).email);
    }

    return response;
}

export const config = {
    matcher: ['/((?!api/auth|_next/static|_next/image|favicon.ico).*)'],
};
```

---

## Matcher

The matcher excludes:

| Excluded path pattern | Reason |
|---|---|
| `api/auth` | NextAuth provider routes — must be publicly accessible |
| `_next/static` | Static build assets |
| `_next/image` | Next.js image optimisation endpoint |
| `favicon.ico` | Browser favicon request |

All other paths are intercepted and require a valid token.

---

## Auth Route Handler

The sign-in flow is handled by the NextAuth provider route at `app/(public)/api/auth/[...nextauth]/route.ts`. The route handler is fixed — it only delegates to `authOptions` from `infra/auth`.

Provider-specific configuration (current provider, environment variables, token flow, and swap instructions) is documented in [Auth Provider](auth-provider.md).

---

## Rules

- `middleware.ts` must live at `src/` root — never move it into a sub-folder.
- Never add business logic to middleware — only authentication checks and header injection.
- Never change the matcher to expose additional routes without an explicit decision.
- `NEXTAUTH_SECRET` must be set via environment variable — never hardcode it.
- The `(public)/api/auth/` route group is the only location for unprotected routes.

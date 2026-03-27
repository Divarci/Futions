import { getToken } from "next-auth/jwt";
import type { NextRequest } from "next/server";
import { NextResponse } from "next/server";

export async function middleware(req: NextRequest) {
    const { pathname, search } = req.nextUrl    
  
    // Validate token
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
}
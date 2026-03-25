# Infra — HTTP Client

The single Axios instance for the entire application. All API files import `httpClient` from here — no file creates its own Axios instance.

---

## File

`infra/http/http-client.ts`

---

## Pattern

```typescript
import axios from "axios";

export const httpClient = axios.create({
    baseURL: process.env.NEXT_PUBLIC_API_BASE_URL,
    headers: { "Content-Type": "application/json" },
});
```

---

## Rules

- One instance for the entire application — never create a second Axios instance.
- Base URL comes from the `NEXT_PUBLIC_API_BASE_URL` environment variable.
- Interceptors (auth headers, error normalization) are added here and nowhere else.

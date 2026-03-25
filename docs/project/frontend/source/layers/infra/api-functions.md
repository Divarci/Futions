# Infra — API Functions

One `.api.ts` file per domain. Each function performs exactly one HTTP call and returns the raw response data. No error handling, no state, no React.

---

## Pattern

```typescript
// infra/{domain}/{domain}.api.ts
import { httpClient } from "@/infra/http";
import type { ApiResponse, PaginatedApiResponse } from "@/core/types";
import type {
    {Entity}ViewModel,
    {Entity}CreateModel,
    {Entity}UpdateModel,
    {Entity}FilterParams,
} from "@/features/{domain}";

const BASE = "/api/v1/{domain}";

export async function get{Entities}(filter: {Entity}FilterParams): Promise<{Entity}ViewModel[]> {
    const response = await httpClient.get<PaginatedApiResponse<{Entity}ViewModel[]>>(BASE, { params: filter });
    return response.data.data;
}

export async function get{Entity}({entity}Id: string): Promise<{Entity}ViewModel> {
    const response = await httpClient.get<ApiResponse<{Entity}ViewModel>>(`${BASE}/${{entity}Id}`);
    return response.data.data;
}

export async function create{Entity}(model: {Entity}CreateModel): Promise<{Entity}ViewModel> {
    const response = await httpClient.post<ApiResponse<{Entity}ViewModel>>(BASE, model);
    return response.data.data;
}

export async function update{Entity}({entity}Id: string, model: {Entity}UpdateModel): Promise<{Entity}ViewModel> {
    const response = await httpClient.patch<ApiResponse<{Entity}ViewModel>>(`${BASE}/${{entity}Id}`, model);
    return response.data.data;
}

export async function delete{Entity}({entity}Id: string): Promise<void> {
    await httpClient.delete(`${BASE}/${{entity}Id}`);
}
```

---

## Rules

- One function per HTTP call — no multi-step operations.
- Functions return raw response data — no transformation, no mapping.
- No error handling here — that belongs in `features/` hooks.
- Function names mirror the HTTP operation: `get`, `create`, `update`, `delete`.

export type ErrorDetails = {
    traceId: string;
    errors: string[];
};

export type Metadata = {
    pageNumber: number;
    pageSize: number;
    totalCount: number;
    pageCount: number;
    totalPages: number;
};

export type ApiResponse<T> = {
    message: string;
    data: T;
    errorDetails: ErrorDetails | null;
};

export type PaginatedApiResponse<T> = ApiResponse<T> & {
    metadata: Metadata;
};

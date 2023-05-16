export interface IPagination {
    readonly pageNumber: number;
    readonly pageSize: number;
    readonly pageCount: number;
    readonly totalCount: number;
}
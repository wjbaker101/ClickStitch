import type { IApiThread } from '@/api/api-models/ApiThread.type';

export interface IGetInventoryResponse {
    readonly threads: Record<number, {
        readonly thread: IApiThread;
        readonly count: number;
    } | null>;
}
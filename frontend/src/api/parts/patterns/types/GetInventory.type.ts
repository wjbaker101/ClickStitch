import type { IApiThread } from '@/api/api-models/ApiThread.type';

export interface IGetInventoryResponse {
    readonly threads: Array<{
        readonly thread: IApiThread;
        readonly count: number;
    }>;
}
import type { IApiThread } from './ApiThread.type';

export interface IGetInventoryThreadsResponse {
    readonly threads: Array<{
        readonly thread: IApiThread;
        readonly count: number;
    }>;
}
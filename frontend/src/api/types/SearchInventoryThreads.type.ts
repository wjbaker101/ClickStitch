import type { IApiThread } from './ApiThread.type';

export interface ISearchInventoryThreadsResponse {
    readonly inventoryThreads: Array<{
        readonly thread: IApiThread;
        readonly count: number;
    }>;
    readonly availableThreads: Array<IApiThread>;
}
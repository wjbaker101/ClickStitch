import type { IThread } from '@/models/Thread.model';

export interface IGetPatternInventory {
    readonly threads: Array<{
        readonly thread: IThread;
        readonly count: number;
    }>;
}
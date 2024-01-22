import type { IThread } from '@/models/Thread.model';

export interface IGetPatternInventory {
    readonly threads: Map<number, IPatternInventoryThread>;
}

export interface IPatternInventoryThread {
    readonly thread: IThread;
    readonly count: number;
}
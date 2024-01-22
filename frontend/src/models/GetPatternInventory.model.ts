import type { IThread } from '@/models/Thread.model';

export interface IGetPatternInventory {
    readonly threads: Map<number, IPatternInventoryThread | null>;
}

export interface IPatternInventoryThread {
    readonly thread: IThread;
    readonly count: number;
}
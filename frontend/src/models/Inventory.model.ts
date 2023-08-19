import type { IThread } from './Thread.model';

export interface IInventoryThread {
    readonly thread: IThread;
    readonly count: number;
}
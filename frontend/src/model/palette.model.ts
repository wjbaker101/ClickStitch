import { IThread } from '@/model/thread.model';

export interface IPalette {
    threads: Map<number, IThread>;
}
import type { IThread } from '@/models/Thread.model';
import type { IApiThread } from '../types/ApiThread.type';

export const threadMapper = {

    map(thread: IApiThread): IThread {
        return {
            reference: thread.reference,
            code: thread.code,
            description: thread.description,
        };
    },

};
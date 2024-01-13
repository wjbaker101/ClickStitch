import type { IThread } from '@/models/Thread.model';
import type { IApiThread } from '@/api/api-models/ApiThread.type';

export const threadMapper = {

    map(thread: IApiThread): IThread {
        return {
            reference: thread.reference,
            brand: thread.brand,
            code: thread.code,
            colour: thread.colour,
        };
    },

};
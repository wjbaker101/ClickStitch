import dayjs from 'dayjs';

import { IPattern, IStitch, IThread } from '@/models/Pattern.model';
import { IApiPattern, IApiStitch, IApiThread } from '../types/ApiPattern.type';

export const patternMapper = {

    map(pattern: IApiPattern): IPattern {
        return {
            reference: pattern.reference,
            createdAt: dayjs(pattern.createdAt),
            title: pattern.title,
            width: pattern.width,
            height: pattern.height,
            price: pattern.price,
            thumbnailUrl: pattern.thumbnailUrl,
        };
    },

    mapStitch(stitch: IApiStitch): IStitch {
        return {
            threadIndex: stitch.threadIndex,
            x: stitch.x,
            y: stitch.y,
        };
    },

    mapThread(thread: IApiThread): IThread {
        return {
            index: thread.index,
            name: thread.name,
            description: thread.description,
            colour: thread.colour,
        };
    },

};
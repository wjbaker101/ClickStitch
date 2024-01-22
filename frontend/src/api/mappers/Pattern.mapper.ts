import dayjs from 'dayjs';

import { type IPattern, type IStitch, type IPatternThread } from '@/models/Pattern.model';
import { type IApiPattern, type IApiStitch, type IApiPatternThread } from '@/api/api-models/ApiPattern.type';
import { creatorMapper } from './Creator.mapper';

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
            threadCount: pattern.threadCount,
            stitchCount: pattern.stitchCount,
            bannerImageUrl: pattern.bannerImageUrl,
            externalShopUrl: pattern.externalShopUrl,
            titleSlug: pattern.titleSlug,
            aidaCount: pattern.aidaCount,
            creator: pattern.creator === null ? null : creatorMapper.map(pattern.creator),
        };
    },

    mapStitch(stitch: IApiStitch): IStitch {
        return {
            threadIndex: stitch.threadIndex,
            x: stitch.x,
            y: stitch.y,
            stitchedAt: stitch.stitchedAt === null ? null : dayjs(stitch.stitchedAt),
        };
    },

    mapThread(thread: IApiPatternThread): IPatternThread {
        return {
            index: thread.index,
            name: thread.name,
            description: thread.description,
            colour: thread.colour,
        };
    },

};
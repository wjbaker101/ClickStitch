import { Dayjs } from 'dayjs';

import type { ICreator } from '@/models/Creator.model';
import type { IUser } from '@/models/User.model';

export interface IPattern {
    readonly reference: string;
    readonly createdAt: Dayjs;
    title: string;
    readonly width: number;
    readonly height: number;
    readonly price: number;
    readonly thumbnailUrl: string;
    readonly threadCount: number;
    readonly stitchCount: number;
    readonly bannerImageUrl: string | null;
    externalShopUrl: string | null;
    readonly titleSlug: string;
    aidaCount: number;
    readonly user: IUser;
    readonly creator: ICreator | null;
}

export interface IStitch {
    readonly threadIndex: number;
    readonly x: number;
    readonly y: number;
    stitchedAt: Dayjs | null;
}

export interface IPatternThread {
    readonly index: number;
    readonly name: string;
    readonly description: string;
    readonly colour: string;
}
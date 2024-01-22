import { Dayjs } from 'dayjs';

import { type ICreator } from './Creator.model';

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
    readonly aidaCount: number;
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
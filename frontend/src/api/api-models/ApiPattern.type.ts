import { type IApiCreator } from '@/api/api-models/ApiCreator.type';
import type { IApiUser } from '@/api/api-models/ApiUser.type';

export interface IApiPattern {
    readonly reference: string;
    readonly createdAt: string;
    readonly title: string;
    readonly width: number;
    readonly height: number;
    readonly price: number;
    readonly thumbnailUrl: string;
    readonly threadCount: number;
    readonly stitchCount: number;
    readonly bannerImageUrl: string | null;
    readonly externalShopUrl: string | null;
    readonly titleSlug: string;
    readonly aidaCount: number;
    readonly user: IApiUser;
    readonly creator: IApiCreator | null;
}

export interface IApiStitch {
    readonly threadIndex: number;
    readonly x: number;
    readonly y: number;
    readonly stitchedAt: string | null;
}

export interface IApiPatternThread {
    readonly index: number;
    readonly name: string;
    readonly description: string;
    readonly colour: string;
}
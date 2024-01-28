import { type IApiPattern } from '@/api/api-models/ApiPattern.type';

export interface IUpdatePatternRequest {
    readonly title: string;
    readonly externalShopUrl: string;
    readonly aidaCount: number;
}

export interface IUpdatePatternResponse {
    readonly pattern: IApiPattern;
}
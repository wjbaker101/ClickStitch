import { type IApiPattern } from '@/api/api-models/ApiPattern.type';

export interface IUpdatePatternRequest {
    readonly title: string;
    readonly externalShopUrl: string;
}

export interface IUpdatePatternResponse {
    readonly pattern: IApiPattern;
}
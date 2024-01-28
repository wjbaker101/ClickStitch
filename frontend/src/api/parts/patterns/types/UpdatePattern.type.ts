import { type IApiPattern } from '@/api/api-models/ApiPattern.type';

export interface IUpdatePatternRequest {
    readonly title: string;
    readonly externalShopUrl: string | null;
    readonly aidaCount: number;
}

export interface IUpdatePatternResponse {
    readonly pattern: IApiPattern;
}
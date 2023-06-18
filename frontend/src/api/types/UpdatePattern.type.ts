import { IApiPattern } from './ApiPattern.type';

export interface IUpdatePatternRequest {
    readonly title: string;
    readonly externalShopUrl: string;
}

export interface IUpdatePatternResponse {
    readonly pattern: IApiPattern;
}
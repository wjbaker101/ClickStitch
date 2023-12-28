import { type IApiPattern } from '@/api/types/ApiPattern.type';

export interface IApiBasketItem {
    readonly pattern: IApiPattern;
    readonly addedAt: string;
}
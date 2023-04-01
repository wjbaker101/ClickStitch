import { IApiPattern } from '@/api/types/ApiPattern.type';

export interface IApiBasket {
    readonly items: Array<IApiBasketItem>;
    readonly totalPrice: number;
}

export interface IApiBasketItem {
    readonly pattern: IApiPattern;
    readonly addedAt: string;
}
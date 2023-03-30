import { Dayjs } from 'dayjs';

import { IPattern } from './Pattern.model';

export interface IBasket {
    readonly items: Array<IBasketItem>;
    readonly totalPrice: number;
}

export interface IBasketItem {
    readonly pattern: IPattern;
    readonly addedAt: Dayjs;
}
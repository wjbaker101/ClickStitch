import { type IApiBasketItem } from '@/api/types/ApiBasket.type';

export interface IAddToBasketResponse {
    readonly item: IApiBasketItem;
}
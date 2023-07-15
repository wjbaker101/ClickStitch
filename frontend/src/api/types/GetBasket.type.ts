import { type IApiBasket } from '@/api/types/ApiBasket.type';

export interface IGetBasketResponse {
    readonly basket: IApiBasket;
}
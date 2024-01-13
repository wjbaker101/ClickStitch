import { type IApiCreator } from '@/api/api-models/ApiCreator.type';

export interface IGetSelfCreator {
    readonly creator: IApiCreator | null;
}
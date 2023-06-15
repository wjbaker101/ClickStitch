import { IApiCreator } from '@/api/types/ApiCreator.type';

export interface IGetSelfCreator {
    readonly creator: IApiCreator | null;
}
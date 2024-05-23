import type { IApiCreator } from '@/api/api-models/ApiCreator.type';

export interface IGetCreatorResponse {
    readonly creator: IApiCreator;
}
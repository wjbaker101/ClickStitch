import { type IApiCreator } from '@/api/api-models/ApiCreator.type';

export interface IUpdateCreatorRequest {
    readonly name: string;
    readonly storeUrl: string;
}

export interface IUpdateCreatorResponse {
    readonly creator: IApiCreator;
}
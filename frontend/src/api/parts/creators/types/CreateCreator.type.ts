import { type IApiCreator } from '@/api/api-models/ApiCreator.type';

export interface ICreateCreatorRequest {
    readonly name: string;
    readonly storeUrl: string;
    readonly description: string;
}

export interface ICreateCreatorResponse {
    readonly creator: IApiCreator;
}
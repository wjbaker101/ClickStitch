import { IApiCreator } from './ApiCreator.type';

export interface ICreateCreatorRequest {
    readonly name: string;
    readonly storeUrl: string;
}

export interface ICreateCreatorResponse {
    readonly creator: IApiCreator;
}
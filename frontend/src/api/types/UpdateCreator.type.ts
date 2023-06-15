import { IApiCreator } from './ApiCreator.type';

export interface IUpdateCreatorRequest {
    readonly name: string;
    readonly storeUrl: string;
}

export interface IUpdateCreatorResponse {
    readonly creator: IApiCreator;
}
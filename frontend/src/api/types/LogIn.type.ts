import { IApiPermission } from './ApiPermission.type';

export interface ILogInRequest {
    readonly email: string;
    readonly password: string;
}

export interface ILogInResponse {
    readonly loginToken: string;
    readonly email: string;
    readonly permissions: Array<IApiPermission>;
}
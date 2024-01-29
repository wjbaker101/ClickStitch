import { type IApiPermission } from '@/api/api-models/ApiPermission.type';

export interface ILogInRequest {
    readonly email: string;
    readonly password: string;
}

export interface ILogInResponse {
    readonly reference: string;
    readonly loginToken: string;
    readonly email: string;
    readonly permissions: Array<IApiPermission>;
}
import { IApiPermission } from './ApiPermission.type';
import { IApiUser } from './ApiUser.type';

export interface IGetSelfResponse {
    readonly user: IApiUser;
    readonly permissions: Array<IApiPermission>;
}
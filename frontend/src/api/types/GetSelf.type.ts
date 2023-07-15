import { type IApiPermission } from './ApiPermission.type';
import { type IApiUser } from './ApiUser.type';

export interface IGetSelfResponse {
    readonly user: IApiUser;
    readonly permissions: Array<IApiPermission>;
}
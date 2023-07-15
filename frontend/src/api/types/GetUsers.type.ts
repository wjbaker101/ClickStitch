import { type IApiUser } from './ApiUser.type';
import { type IApiPagination } from './ApiPagination.type';
import { type IApiPermission } from './ApiPermission.type';

export interface IGetUsersResponse {
    readonly users: Array<IUserDetails>;
    readonly pagination: IApiPagination;
}

export interface IUserDetails {
    readonly user: IApiUser;
    readonly permissions: Array<IApiPermission>;
}
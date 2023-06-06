import { IApiUser } from './ApiUser.type';
import { IApiPagination } from './ApiPagination.type';
import { IApiPermission } from './ApiPermission.type';

export interface IGetUsersResponse {
    readonly users: Array<IUserDetails>;
    readonly pagination: IApiPagination;
}

export interface IUserDetails {
    readonly user: IApiUser;
    readonly permissions: Array<IApiPermission>;
}
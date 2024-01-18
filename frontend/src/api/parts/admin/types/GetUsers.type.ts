import { type IApiUser } from '@/api/api-models/ApiUser.type';
import { type IApiPagination } from '@/api/api-models/ApiPagination.type';
import { type IApiPermission } from '@/api/api-models/ApiPermission.type';

export interface IGetUsersResponse {
    readonly users: Array<IUserDetails>;
    readonly pagination: IApiPagination;
}

export interface IUserDetails {
    readonly user: IApiUser;
    readonly permissions: Array<IApiPermission>;
}
import { IApiUser } from './ApiUser.type';
import { IApiPagination } from './ApiPagination.type';

export interface IGetUsersResponse {
    readonly users: Array<IApiUser>;
    readonly pagination: IApiPagination;
}
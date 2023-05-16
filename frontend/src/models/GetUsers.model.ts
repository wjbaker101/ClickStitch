import { IUser } from './User.model';
import { IPagination } from './Pagination.model';

export interface IGetUsers {
    readonly users: Array<IUser>;
    readonly pagination: IPagination;
}
import { type IUser } from './User.model';
import { type IPagination } from './Pagination.model';
import { type IPermission } from './Permission.model';

export interface IGetUsers {
    readonly users: Array<IUserDetails>;
    readonly pagination: IPagination;
}

export interface IUserDetails {
    readonly user: IUser;
    readonly permissions: Array<IPermission>;
}
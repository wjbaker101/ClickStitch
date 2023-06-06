import { IUser } from './User.model';
import { IPagination } from './Pagination.model';
import { IPermission } from './Permission.model';

export interface IGetUsers {
    readonly users: Array<IUserDetails>;
    readonly pagination: IPagination;
}

export interface IUserDetails {
    readonly user: IUser;
    readonly permissions: Array<IPermission>;
}
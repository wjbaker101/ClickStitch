import { IPermission } from '@/models/Permission.model';
import { IUser } from '@/models/User.model';

export interface IGetSelf {
    readonly user: IUser;
    readonly permissions: Array<IPermission>;
}
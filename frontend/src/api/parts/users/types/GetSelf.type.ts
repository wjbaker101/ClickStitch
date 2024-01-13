import { type IApiPermission } from '@/api/api-models/ApiPermission.type';
import { type IApiUser } from '@/api/api-models/ApiUser.type';

export interface IGetSelfResponse {
    readonly user: IApiUser;
    readonly permissions: Array<IApiPermission>;
}
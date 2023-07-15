import { type IApiPermission } from './ApiPermission.type';

export interface IGetPermissionsResponse {
    readonly permissions: Array<IApiPermission>;
}
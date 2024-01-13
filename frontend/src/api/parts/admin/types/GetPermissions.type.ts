import { type IApiPermission } from '@/api/api-models/ApiPermission.type';

export interface IGetPermissionsResponse {
    readonly permissions: Array<IApiPermission>;
}
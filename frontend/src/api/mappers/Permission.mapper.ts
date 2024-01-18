import { type IPermission, type PermissionType } from '@/models/Permission.model';
import { type IApiPermission } from '@/api/api-models/ApiPermission.type';

export const permissionMapper = {

    map(permission: IApiPermission): IPermission {
        return {
            type: permissionMapper.mapType(permission.type),
            name: permission.name,
        };
    },

    mapType(type: number): PermissionType {
        switch (type) {
            case 1:
                return 'Admin';
            case 2:
                return 'Creator';
            default:
                throw new Error(`Unable to map permission type: '${type}.`);
        }
    },

    mapTypeToApi(type: PermissionType): number {
        switch (type) {
            case 'Admin':
                return 1;
            case 'Creator':
                return 2;
            default:
                throw new Error(`Unable to map permission type: '${type}.`);
        }
    },

};
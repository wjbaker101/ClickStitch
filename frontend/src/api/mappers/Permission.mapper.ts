import { IPermission, PermissionType } from '@/models/Permission.model';
import { IApiPermission } from '../types/ApiPermission.type';

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

};
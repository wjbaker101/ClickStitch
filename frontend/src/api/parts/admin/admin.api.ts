import { apiClient } from '@/api/client';

import { paginationMapper } from '@/api/mappers/Pagination.mapper';
import { userMapper } from '@/api/mappers/User.mapper';
import { permissionMapper } from '@/api/mappers/Permission.mapper';

import type { IPermission } from '@/models/Permission.model';
import type { IGetUsers } from '@/models/GetUsers.model';

import type { IGetUsersResponse } from '@/api/parts/admin/types/GetUsers.type';
import type { IGetPermissionsResponse } from '@/api/parts/admin/types/GetPermissions.type';
import type { IAssignPermissionToUserRequest, IAssignPermissionToUserResponse } from '@/api/parts/admin/types/AssignPermissionToUser.type';
import type { IRemovePermissionFromUserResponse } from '@/api/parts/admin/types/RemovePermissionFromUser.type';

export const adminApi = {

    async getUsers(pageNumber: number, pageSize: number): Promise<IGetUsers | Error> {
        const queryParams = new Map<string, string>();
        queryParams.set('page_number', String(pageNumber));
        queryParams.set('page_size', String(pageSize));

        const response = await apiClient.get<IGetUsersResponse>({
            url: '/admin/users',
            queryParams,
            auth: {
                use: true,
                required: true,
            },
        });

        if (response instanceof Error)
            return response;

        return {
            users: response.users.map(x => ({
                user: userMapper.map(x.user),
                permissions: x.permissions.map(permissionMapper.map),
            })),
            pagination: paginationMapper.map(response.pagination),
        };
    },

    async getPermissions(): Promise<Array<IPermission> | Error> {
        const response = await apiClient.get<IGetPermissionsResponse>({
            url: '/admin/permissions',
            auth: {
                use: true,
                required: true,
            },
        });

        if (response instanceof Error)
            return response;

        return response.permissions.map(permissionMapper.map);
    },

    async assignPermissionToUser(userReference: string, request: IAssignPermissionToUserRequest): Promise<void | Error> {
        const response = await apiClient.post<IAssignPermissionToUserResponse>({
            url: `/admin/users/${userReference}/permissions`,
            body: request,
            auth: {
                use: true,
                required: true,
            },
        });

        if (response instanceof Error)
            return response;
    },

    async removePermissionFromUser(userReference: string, permissionType: number): Promise<void | Error> {
        const response = await apiClient.delete<IRemovePermissionFromUserResponse>({
            url: `/admin/users/${userReference}/permissions/${permissionType}`,
            auth: {
                use: true,
                required: true,
            },
        });

        if (response instanceof Error)
            return response;
    },

};
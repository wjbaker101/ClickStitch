import { client } from '@/api/client';

import { useAuth } from '@/use/auth/Auth.use';
import { ApiErrorMapper } from '@/api/ApiErrorMapper';

import { paginationMapper } from '@/api/mappers/Pagination.mapper';
import { userMapper } from '@/api/mappers/User.mapper';
import { permissionMapper } from '@/api/mappers/Permission.mapper';
import { threadMapper } from '@/api/mappers/Thread.mapper';

import type { IPermission } from '@/models/Permission.model';
import type { IGetUsers } from '@/models/GetUsers.model';
import type { IThread } from '@/models/Thread.model';

import type { IApiResultResponse } from '@/api/api-models/ApiResponse.type';

import type { IGetUsersResponse } from '@/api/parts/admin/types/GetUsers.type';
import type { IGetPermissionsResponse } from '@/api/parts/admin/types/GetPermissions.type';
import type { IAssignPermissionToUserRequest, IAssignPermissionToUserResponse } from '@/api/parts/admin/types/AssignPermissionToUser.type';
import type { IRemovePermissionFromUserResponse } from '@/api/parts/admin/types/RemovePermissionFromUser.type';
import type { ICreateThreadRequest, ICreateThreadResponse } from '@/api/parts/admin/types/CreateThread.type';
import type { IUpdateThreadRequest, IUpdateThreadResponse } from '@/api/parts/admin/types/UpdateThread.type';
import type { IDeleteThreadResponse } from '@/api/parts/admin/types/DeleteThread.type';

const auth = useAuth();

export const adminApi = {

    async getUsers(pageNumber: number, pageSize: number): Promise<IGetUsers | Error> {
        if (auth.details.value === null)
            return new Error('You must be logged in for this action.');

        const url = `/admin/users?page_number=${pageNumber}&page_size=${pageSize}`;

        try {
            const response = await client.get<IApiResultResponse<IGetUsersResponse>>(url, {
                headers: {
                    'Authorization': `Bearer ${auth.details.value.loginToken}`,
                },
            });

            const result = response.data.result;

            return {
                users: result.users.map(x => ({
                    user: userMapper.map(x.user),
                    permissions: x.permissions.map(permissionMapper.map),
                })),
                pagination: paginationMapper.map(result.pagination),
            };
        }
        catch (error) {
            return ApiErrorMapper.map(error);
        }
    },

    async getPermissions(): Promise<Array<IPermission> | Error> {
        if (auth.details.value === null)
            return new Error('You must be logged in for this action.');

        try {
            const response = await client.get<IApiResultResponse<IGetPermissionsResponse>>('/admin/permissions', {
                headers: {
                    'Authorization': `Bearer ${auth.details.value.loginToken}`,
                },
            });

            const result = response.data.result;

            return result.permissions.map(permissionMapper.map);
        }
        catch (error) {
            return ApiErrorMapper.map(error);
        }
    },

    async assignPermissionToUser(userReference: string, request: IAssignPermissionToUserRequest): Promise<void | Error> {
        if (auth.details.value === null)
            return new Error('You must be logged in for this action.');

        const url = `/admin/users/${userReference}/permissions`;

        try {
            const response = await client.post<IApiResultResponse<IAssignPermissionToUserResponse>>(url, request, {
                headers: {
                    'Authorization': `Bearer ${auth.details.value.loginToken}`,
                },
            });

            const result = response.data.result;
        }
        catch (error) {
            return ApiErrorMapper.map(error);
        }
    },

    async removePermissionFromUser(userReference: string, permissionType: number): Promise<void | Error> {
        if (auth.details.value === null)
            return new Error('You must be logged in for this action.');

        const url = `/admin/users/${userReference}/permissions/${permissionType}`;

        try {
            const response = await client.delete<IApiResultResponse<IRemovePermissionFromUserResponse>>(url, {
                headers: {
                    'Authorization': `Bearer ${auth.details.value.loginToken}`,
                },
            });

            const result = response.data.result;
        }
        catch (error) {
            return ApiErrorMapper.map(error);
        }
    },

    async createThread(request: ICreateThreadRequest): Promise<IThread | Error> {
        if (auth.details.value === null)
            return new Error('You must be logged in for this action.');

        try {
            const response = await client.post<IApiResultResponse<ICreateThreadResponse>>('/admin/threads', request, {
                headers: {
                    'Authorization': `Bearer ${auth.details.value.loginToken}`,
                },
            });

            const thread = response.data.result.thread;

            return threadMapper.map(thread);
        }
        catch (error) {
            return ApiErrorMapper.map(error);
        }
    },

    async updateThread(threadReference: string, request: IUpdateThreadRequest): Promise<IThread | Error> {
        if (auth.details.value === null)
            return new Error('You must be logged in for this action.');

        try {
            const response = await client.put<IApiResultResponse<IUpdateThreadResponse>>(`/admin/threads/${threadReference}`, request, {
                headers: {
                    'Authorization': `Bearer ${auth.details.value.loginToken}`,
                },
            });

            const thread = response.data.result.thread;

            return threadMapper.map(thread);
        }
        catch (error) {
            return ApiErrorMapper.map(error);
        }
    },

    async deleteThread(threadReference: string): Promise<void | Error> {
        if (auth.details.value === null)
            return new Error('You must be logged in for this action.');

        try {
            const response = await client.post<IApiResultResponse<IDeleteThreadResponse>>(`/admin/threads/${threadReference}`, {
                headers: {
                    'Authorization': `Bearer ${auth.details.value.loginToken}`,
                },
            });

            const result = response.data.result;
        }
        catch (error) {
            return ApiErrorMapper.map(error);
        }
    },

};
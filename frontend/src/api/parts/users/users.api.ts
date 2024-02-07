import { apiClient } from '@/api/client';

import { userMapper } from '@/api/mappers/User.mapper';
import { permissionMapper } from '@/api/mappers/Permission.mapper';

import type { IGetSelf } from '@/models/GetSelf.model';

import type { ICreateUserRequest, ICreateUserResponse } from '@/api/parts/users/types/CreateUser.type';
import type { IGetSelfResponse } from '@/api/parts/users/types/GetSelf.type';

export const usersApi = {

    async getSelf(): Promise<IGetSelf | Error> {
        const response = await apiClient.get<IGetSelfResponse>({
            url: '/users/self',
            auth: {
                required: true,
                use: true,
            },
        });

        if (response instanceof Error)
            return response;

        return {
            user: userMapper.map(response.user),
            permissions: response.permissions.map(permissionMapper.map),
        };
    },

    async createUser(request: ICreateUserRequest): Promise<ICreateUserResponse | Error> {
        const response = await apiClient.post<ICreateUserResponse>({
            url: '/users',
            body: request,
            auth: {
                required: false,
                use: false,
            },
        });

        if (response instanceof Error)
            return response;

        return {};
    },

};
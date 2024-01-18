import { client } from '@/api/client';

import { useAuth } from '@/use/auth/Auth.use';
import { ApiErrorMapper } from '@/api/ApiErrorMapper';

import { userMapper } from '@/api/mappers/User.mapper';
import { permissionMapper } from '@/api/mappers/Permission.mapper';

import type { IApiResultResponse } from '@/api/api-models/ApiResponse.type';

import type { IGetSelf } from '@/models/GetSelf.model';

import type { ICreateUserRequest, ICreateUserResponse } from '@/api/parts/users/types/CreateUser.type';
import type { IGetSelfResponse } from '@/api/parts/users/types/GetSelf.type';

const auth = useAuth();

export const usersApi = {

    async getSelf(): Promise<IGetSelf | Error> {
        if (auth.details.value === null)
            return new Error('You must be logged in for this action.');

        try {
            const response = await client.get<IApiResultResponse<IGetSelfResponse>>('/users/self', {
                headers: {
                    'Authorization': `Bearer ${auth.details.value.loginToken}`,
                },
            });

            const result = response.data.result;

            return {
                user: userMapper.map(result.user),
                permissions: result.permissions.map(permissionMapper.map),
            };
        }
        catch (error) {
            return ApiErrorMapper.map(error);
        }
    },

    async createUser(request: ICreateUserRequest): Promise<ICreateUserResponse | Error> {
        try {
            const response = await client.post<IApiResultResponse<ICreateUserResponse>>('/users', request);

            return {};
        }
        catch (error) {
            return ApiErrorMapper.map(error);
        }
    },

};
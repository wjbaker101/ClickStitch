import dayjs from 'dayjs';

import { client } from '@/api/client';

import { type IAuth } from '@/use/auth/Auth.use';
import { ApiErrorMapper } from '@/api/ApiErrorMapper';

import { permissionMapper } from '@/api/mappers/Permission.mapper';

import type { IApiResultResponse } from '@/api/api-models/ApiResponse.type';

import type { ILogInRequest, ILogInResponse } from '@/api/parts/auth/types/LogIn.type';

export const authApi = {

    async logIn(request: ILogInRequest): Promise<IAuth | Error> {
        try {
            const response = await client.post<IApiResultResponse<ILogInResponse>>('/auth/log_in', request);

            const result = response.data.result;

            return {
                loginToken: result.loginToken,
                email: result.email,
                permissions: result.permissions.map(permissionMapper.map),
                loggedInAt: dayjs(response.data.responseAt),
            };
        }
        catch (error) {
            return ApiErrorMapper.map(error);
        }
    },

};
import dayjs from 'dayjs';

import { apiClient } from '@/api/client';

import { type IAuth } from '@/use/auth/Auth.use';

import { permissionMapper } from '@/api/mappers/Permission.mapper';

import type { ILogInRequest, ILogInResponse } from '@/api/parts/auth/types/LogIn.type';

export const authApi = {

    async logIn(request: ILogInRequest): Promise<IAuth | Error> {
        const response = await apiClient.post<ILogInResponse>({
            url: '/auth/log_in',
            body: request,
            auth: {
                required: false,
                use: false,
            },
        });

        if (response instanceof Error)
            return response;

        return {
            reference: response.reference,
            loginToken: response.loginToken,
            email: response.email,
            permissions: response.permissions.map(permissionMapper.map),
            loggedInAt: dayjs(),
        };
    },

};
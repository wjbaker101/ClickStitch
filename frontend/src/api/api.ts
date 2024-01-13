import axios from 'axios';

import { useAuth } from '@/use/auth/Auth.use';
import { ApiErrorMapper } from '@/api/ApiErrorMapper';

import { threadMapper } from '@/api/mappers/Thread.mapper';

import type { IApiResultResponse } from '@/api/api-models/ApiResponse.type';

import type { IInventoryThread } from '@/models/Inventory.model';
import type { IGetInventoryThreadsResponse } from './types/GetInventoryThreads.type';
import type { IUpdateInventoryThreadRequest, IUpdateInventoryThreadResponse } from './types/UpdateInventoryThread.type';
import type { ISearchInventoryThreadsResponse } from './types/SearchInventoryThreads.type';

import { adminApi } from '@/api/parts/admin/admin.api';
import { authApi } from '@/api/parts/auth/auth.api';
import { creatorsApi } from '@/api/parts/creators/creators.api';
import { patternsApi } from '@/api/parts/patterns/patterns.api';
import { projectsApi } from '@/api/parts/projects/projects.api';
import { usersApi } from '@/api/parts/users/users.api';

const auth = useAuth();

const client = axios.create({
    baseURL: '/api',
});

export const api = {

    admin: adminApi,

    auth: authApi,

    creators: creatorsApi,

    patterns: patternsApi,

    projects: projectsApi,

    users: usersApi,

    inventory: {

        async searchThreads(searchTerm: string, brand: string | null): Promise<ISearchInventoryThreadsResponse | Error> {
            if (auth.details.value === null)
                return new Error('You must be logged in for this action.');

            const url = new URL('/api/inventory/threads/search', window.location.origin);

            if (searchTerm.length > 0)
                url.searchParams.set('search_term', searchTerm);

            if (brand !== null)
                url.searchParams.set('brand', brand);

            try {
                const response = await client.get<IApiResultResponse<ISearchInventoryThreadsResponse>>(url.toString(), {
                    headers: {
                        'Authorization': `Bearer ${auth.details.value.loginToken}`,
                    },
                });

                return response.data.result;
            }
            catch (error) {
                return ApiErrorMapper.map(error);
            }
        },

        async getThreads(): Promise<Array<IInventoryThread> | Error> {
            if (auth.details.value === null)
                return new Error('You must be logged in for this action.');

            try {
                const response = await client.get<IApiResultResponse<IGetInventoryThreadsResponse>>(`/inventory/threads`, {
                    headers: {
                        'Authorization': `Bearer ${auth.details.value.loginToken}`,
                    },
                });

                const threads = response.data.result.threads;

                return threads.map(x => ({
                    thread: threadMapper.map(x.thread),
                    count: x.count,
                }));
            }
            catch (error) {
                return ApiErrorMapper.map(error);
            }
        },

        async updateThread(threadReference: string, request: IUpdateInventoryThreadRequest): Promise<void | Error> {
            if (auth.details.value === null)
                return new Error('You must be logged in for this action.');

            try {
                const response = await client.put<IApiResultResponse<IUpdateInventoryThreadResponse>>(`/inventory/threads/${threadReference}`, request, {
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

    },

};
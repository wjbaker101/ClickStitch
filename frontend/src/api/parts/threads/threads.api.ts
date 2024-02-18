import { apiClient } from '@/api/client';

import { threadMapper } from '@/api/mappers/Thread.mapper';

import type { IThread } from '@/models/Thread.model';

import type {
    IGetThreadsByColourRequest,
    IGetThreadsByColourResponse,
} from '@/api/parts/threads/types/GetThreadsByColour.type';

export const threadsApi = {

    async getByColour(request: IGetThreadsByColourRequest): Promise<Array<IThread> | Error> {
        const response = await apiClient.post<IGetThreadsByColourResponse>({
            url: '/threads/by-colour',
            body: request,
            auth: {
                required: true,
                use: true,
            },
        });

        if (response instanceof Error)
            return response;

        return response.threads.map(threadMapper.map);
    },

};
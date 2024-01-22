import { apiClient, client } from '@/api/client';

import { useAuth } from '@/use/auth/Auth.use';
import { ApiErrorMapper } from '@/api/ApiErrorMapper';

import { patternMapper } from '@/api/mappers/Pattern.mapper';
import { threadMapper } from '@/api/mappers/Thread.mapper';

import type { IApiResultResponse } from '@/api/api-models/ApiResponse.type';

import type { IDeletePattern } from '@/models/DeletePattern.model';
import type { IPattern } from '@/models/Pattern.model';
import type { IGetPatternInventory, IPatternInventoryThread } from '@/models/GetPatternInventory.model';

import type { ISearchPatternsResponse } from '@/api/parts/patterns/types/SearchPatterns.type';
import type { IUpdatePatternRequest, IUpdatePatternResponse } from '@/api/parts/patterns/types/UpdatePattern.type';
import type { ICreatePatternRequest, ICreatePatternResponse } from '@/api/parts/patterns/types/CreatePattern.type';
import type { IDeletePatternResponse } from '@/api/parts/patterns/types/DeletePattern.type';
import type { IGetInventoryResponse } from '@/api/parts/patterns/types/GetInventory.type';

const auth = useAuth();

export const patternsApi = {

    async search(): Promise<Array<IPattern> | Error> {
        try {
            const response = await client.get<IApiResultResponse<ISearchPatternsResponse>>('/patterns', {
                headers: (auth.details.value === null) ? {} : {
                    'Authorization': `Bearer ${auth.details.value.loginToken}`,
                },
            });

            const patterns = response.data.result.patterns;

            return patterns.map(patternMapper.map);
        }
        catch (error) {
            return ApiErrorMapper.map(error);
        }
    },

    async update(patternReference: string, request: IUpdatePatternRequest): Promise<IPattern | Error> {
        if (auth.details.value === null)
            return new Error('You must be logged in for this action.');

        try {
            const response = await client.put<IApiResultResponse<IUpdatePatternResponse>>(`/patterns/${patternReference}`, request, {
                headers: {
                    'Authorization': `Bearer ${auth.details.value.loginToken}`,
                },
            });

            const result = response.data.result;

            return patternMapper.map(result.pattern);
        }
        catch (error) {
            return ApiErrorMapper.map(error);
        }
    },

    async create(bannerImage: File, patternData: string, request: ICreatePatternRequest): Promise<void | Error> {
        if (auth.details.value === null)
            return new Error('You must be logged in for this action.');

        try {
            const formData = new FormData();
            formData.append('banner_image', bannerImage);
            formData.append('request_body', JSON.stringify(request));
            formData.append('pattern_data', patternData);

            const response = await client.post<IApiResultResponse<ICreatePatternResponse>>('/patterns', formData, {
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

    async verify(patternData: string): Promise<boolean | Error> {
        if (auth.details.value === null)
            return new Error('You must be logged in for this action.');

        try {
            const formData = new FormData();
            formData.append('pattern_data', patternData);

            const response = await client.post<IApiResultResponse<{}>>('/patterns/verify', formData, {
                headers: {
                    'Authorization': `Bearer ${auth.details.value.loginToken}`,
                },
            });

            const result = response.data.result;

            return true;
        }
        catch (error) {
            return false;
        }
    },

    async delete(patternReference: string): Promise<IDeletePattern | Error> {
        if (auth.details.value === null)
            return new Error('You must be logged in for this action.');

        try {
            const response = await client.delete<IApiResultResponse<IDeletePatternResponse>>(`/patterns/${patternReference}`, {
                headers: {
                    'Authorization': `Bearer ${auth.details.value.loginToken}`,
                },
            });

            const result = response.data.result;

            return {
                message: result.message,
            };
        }
        catch (error) {
            return ApiErrorMapper.map(error);
        }
    },

    async getInventory(patternReference: string): Promise<IGetPatternInventory | Error> {
        const response = await apiClient.get<IGetInventoryResponse>({
            url: `/patterns/${patternReference}/inventory`,
            auth: {
                use: true,
                required: true,
            },
        });

        if (response instanceof Error)
            return response;

        const map = new Map<number, IPatternInventoryThread>();

        for (const threadIndex in response.threads) {
            const thread = response.threads[Number(threadIndex)]
            map.set(Number(threadIndex), {
                thread: threadMapper.map(thread.thread),
                count: thread.count,
            })
        }

        return {
            threads: map,
        };
    },

};
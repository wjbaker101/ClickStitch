import { apiClient } from '@/api/client';

import { patternMapper } from '@/api/mappers/Pattern.mapper';
import { threadMapper } from '@/api/mappers/Thread.mapper';

import type { IDeletePattern } from '@/models/DeletePattern.model';
import type { IPattern } from '@/models/Pattern.model';
import type { IGetPatternInventory, IPatternInventoryThread } from '@/models/GetPatternInventory.model';

import type { ISearchPatternsResponse } from '@/api/parts/patterns/types/SearchPatterns.type';
import type { IUpdatePatternRequest, IUpdatePatternResponse } from '@/api/parts/patterns/types/UpdatePattern.type';
import type { ICreatePatternRequest, ICreatePatternResponse } from '@/api/parts/patterns/types/CreatePattern.type';
import type { IDeletePatternResponse } from '@/api/parts/patterns/types/DeletePattern.type';
import type { IGetInventoryResponse } from '@/api/parts/patterns/types/GetInventory.type';
import type { IGetPatternResponse } from '@/api/parts/patterns/types/GetPattern.type';

export const patternsApi = {

    async get(patternReference: string): Promise<IPattern | Error> {
        const response = await apiClient.get<IGetPatternResponse>({
            url: `/patterns/${patternReference}`,
            auth: {
                required: false,
                use: true,
            },
        });

        if (response instanceof Error)
            return response;

        return patternMapper.map(response.pattern);
    },

    async search(): Promise<Array<IPattern> | Error> {
        const response = await apiClient.get<ISearchPatternsResponse>({
            url: '/patterns',
            auth: {
                required: false,
                use: true,
            },
        });

        if (response instanceof Error)
            return response;

        const patterns = response.patterns;

        return patterns.map(patternMapper.map);
    },

    async update(patternReference: string, request: IUpdatePatternRequest): Promise<IPattern | Error> {
        const response = await apiClient.put<IUpdatePatternResponse>({
            url: `/patterns/${patternReference}`,
            body: request,
            auth: {
                required: true,
                use: true,
            },
        });

        if (response instanceof Error)
            return response;
        
        return patternMapper.map(response.pattern);
    },

    async create(bannerImage: File, patternData: string, request: ICreatePatternRequest): Promise<void | Error> {
        const formData = new FormData();
        formData.append('banner_image', bannerImage);
        formData.append('request_body', JSON.stringify(request));
        formData.append('pattern_data', patternData);

        const response = await apiClient.post<ICreatePatternResponse>({
            url: '/patterns',
            body: formData,
            auth: {
                required: true,
                use: true,
            },
        });

        if (response instanceof Error)
            return response;
    },

    async verify(patternData: string): Promise<boolean> {
        const formData = new FormData();
        formData.append('pattern_data', patternData);

        const response = await apiClient.post<void>({
            url: '/patterns/verify',
            body: formData,
            auth: {
                required: true,
                use: true,
            },
        });

        if (response instanceof Error)
            return false;

        return true;
    },

    async delete(patternReference: string): Promise<IDeletePattern | Error> {
        const response = await apiClient.delete<IDeletePatternResponse>({
            url: `/patterns/${patternReference}`,
            auth: {
                required: true,
                use: true,
            },
        });

        if (response instanceof Error)
            return response;

        return {
            message: response.message,
        };
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

        const map = new Map<number, IPatternInventoryThread | null>();

        for (const threadIndex in response.threads) {
            const thread = response.threads[Number(threadIndex)]

            if (thread === null) {
                map.set(Number(threadIndex), null);
                continue;
            }

            map.set(Number(threadIndex), {
                thread: threadMapper.map(thread.thread),
                count: thread.count,
            });
        }

        return {
            threads: map,
        };
    },

};
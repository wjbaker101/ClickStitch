import { apiClient } from '@/api/client';

import { paginationMapper } from '@/api/mappers/Pagination.mapper';
import { patternMapper } from '@/api/mappers/Pattern.mapper';
import { creatorMapper } from '@/api/mappers/Creator.mapper';

import type { IGetCreatorPatterns } from '@/models/GetCreatorPatterns.model';
import type { ICreator } from '@/models/Creator.model';

import type { ICreateCreatorRequest, ICreateCreatorResponse } from '@/api/parts/creators/types/CreateCreator.type';
import type { IUpdateCreatorRequest, IUpdateCreatorResponse } from '@/api/parts/creators/types/UpdateCreator.type';
import type { IGetSelfCreator } from '@/api/parts/creators/types/GetSelfCreator.type';
import type { IGetCreatorPatternsResponse } from '@/api/parts/creators/types/GetCreatorPatterns.type';

export const creatorsApi = {

    async createCreator(request: ICreateCreatorRequest): Promise<ICreator | Error> {
        const response = await apiClient.post<ICreateCreatorResponse>({
            url: '/creators',
            body: request,
            auth: {
                required: true,
                use: true,
            },
        });

        if (response instanceof Error)
            return response;

        return creatorMapper.map(response.creator);
    },

    async updateCreator(creatorReference: string, request: IUpdateCreatorRequest): Promise<ICreator | Error> {
        const response = await apiClient.put<IUpdateCreatorResponse>({
            url: `/creators/${creatorReference}`,
            body: request,
            auth: {
                required: true,
                use: true,
            },
        });

        if (response instanceof Error)
            return response;

        return creatorMapper.map(response.creator);
    },

    async getSelf(): Promise<ICreator | null | Error> {
        const response = await apiClient.get<IGetSelfCreator>({
            url: '/creators/self',
            auth: {
                required: true,
                use: true,
            },
        });

        if (response instanceof Error)
            return response;

        if (response.creator === null)
            return null;

        return creatorMapper.map(response.creator);
    },

    async getPatterns(creatorReference: string, pageSize: number, pageNumber: number): Promise<IGetCreatorPatterns | Error> {
        const response = await apiClient.get<IGetCreatorPatternsResponse>({
            url: `/creators/${creatorReference}/patterns?page_size=${pageSize}&page_number=${pageNumber}`,
            auth: {
                required: true,
                use: true,
            },
        });

        if (response instanceof Error)
            return response;

        return {
            patterns: response.patterns.map(patternMapper.map),
            pagination: paginationMapper.map(response.pagination),
        };
    },
};
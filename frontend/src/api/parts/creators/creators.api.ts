import { client } from '@/api/client';

import { useAuth } from '@/use/auth/Auth.use';
import { ApiErrorMapper } from '@/api/ApiErrorMapper';

import { paginationMapper } from '@/api/mappers/Pagination.mapper';
import { patternMapper } from '@/api/mappers/Pattern.mapper';
import { creatorMapper } from '@/api/mappers/Creator.mapper';

import type { IApiResultResponse } from '@/api/api-models/ApiResponse.type';

import type { IGetCreatorPatterns } from '@/models/GetCreatorPatterns.model';
import type { ICreator } from '@/models/Creator.model';

import type { ICreateCreatorRequest, ICreateCreatorResponse } from '@/api/parts/creators/types/CreateCreator.type';
import type { IUpdateCreatorRequest, IUpdateCreatorResponse } from '@/api/parts/creators/types/UpdateCreator.type';
import type { IGetSelfCreator } from '@/api/parts/creators/types/GetSelfCreator.type';
import type { IGetCreatorPatternsResponse } from '@/api/parts/creators/types/GetCreatorPatterns.type';

const auth = useAuth();

export const creatorsApi = {

    async createCreator(request: ICreateCreatorRequest): Promise<ICreator | Error> {
        if (auth.details.value === null)
            return new Error('You must be logged in for this action.');

        try {
            const response = await client.post<IApiResultResponse<ICreateCreatorResponse>>('/creators', request, {
                headers: {
                    'Authorization': `Bearer ${auth.details.value.loginToken}`,
                },
            });

            const result = response.data.result;

            return creatorMapper.map(result.creator);
        }
        catch (error) {
            return ApiErrorMapper.map(error);
        }
    },

    async updateCreator(creatorReference: string, request: IUpdateCreatorRequest): Promise<ICreator | Error> {
        if (auth.details.value === null)
            return new Error('You must be logged in for this action.');

        try {
            const response = await client.put<IApiResultResponse<IUpdateCreatorResponse>>(`/creators/${creatorReference}`, request, {
                headers: {
                    'Authorization': `Bearer ${auth.details.value.loginToken}`,
                },
            });

            const result = response.data.result;

            return creatorMapper.map(result.creator);
        }
        catch (error) {
            return ApiErrorMapper.map(error);
        }
    },

    async getSelf(): Promise<ICreator | null | Error> {
        if (auth.details.value === null)
            return new Error('You must be logged in for this action.');

        try {
            const response = await client.get<IApiResultResponse<IGetSelfCreator>>('/creators/self', {
                headers: {
                    'Authorization': `Bearer ${auth.details.value.loginToken}`,
                },
            });

            const result = response.data.result;

            if (result.creator === null)
                return null;

            return creatorMapper.map(result.creator);
        }
        catch (error) {
            return ApiErrorMapper.map(error);
        }
    },

    async getPatterns(creatorReference: string, pageSize: number, pageNumber: number): Promise<IGetCreatorPatterns | Error> {
        if (auth.details.value === null)
            return new Error('You must be logged in for this action.');

        const url = `/creators/${creatorReference}/patterns?page_size=${pageSize}&page_number=${pageNumber}`;

        try {
            const response = await client.get<IApiResultResponse<IGetCreatorPatternsResponse>>(url, {
                headers: {
                    'Authorization': `Bearer ${auth.details.value.loginToken}`,
                },
            });

            const result = response.data.result;

            return {
                patterns: result.patterns.map(patternMapper.map),
                pagination: paginationMapper.map(result.pagination),
            };
        }
        catch (error) {
            return ApiErrorMapper.map(error);
        }
    },
};
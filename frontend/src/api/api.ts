import axios from 'axios';
import dayjs from 'dayjs';

import { useAuth } from '@/use/auth/Auth.use';
import { ApiErrorMapper } from '@/api/ApiErrorMapper';

import { userMapper } from '@/api/mappers/User.mapper';
import { permissionMapper } from '@/api/mappers/Permission.mapper';
import { patternMapper } from '@/api/mappers/Pattern.mapper';
import { projectMapper } from '@/api/mappers/Project.mapper';
import { threadMapper } from '@/api/mappers/Thread.mapper';

import type { IApiResultResponse } from '@/api/api-models/ApiResponse.type';

import type { IDeletePattern } from '@/models/DeletePattern.model';
import type { IGetAnalytics } from '@/models/GetAnalytics.model';
import type { IProject } from '@/models/Project.model';
import type { IGetProject } from '@/models/GetProject.model';
import type { IPattern } from '@/models/Pattern.model';
import type { IGetSelf } from '@/models/GetSelf.model';

import type { ICreateUserRequest, ICreateUserResponse } from '@/api/types/CreateUser.type';
import type { ISearchPatternsResponse } from '@/api/types/SearchPatterns.type';
import type { IGetProjectsResponse } from '@/api/types/GetProjects.type';
import type { IGetProjectResponse } from '@/api/types/GetProject.type';
import type { ICompleteStitchesRequest } from '@/api/types/CompleteStitches.type';
import type { IGetAnalyticsResponse } from '@/api/types/GetAnalytics.type';
import type { IGetSelfResponse } from '@/api/types/GetSelf.type';
import type { IUpdatePatternRequest, IUpdatePatternResponse } from '@/api/types/UpdatePattern.type';
import type { ICreatePatternRequest, ICreatePatternResponse } from '@/api/types/CreatePattern.type';
import type { IDeletePatternResponse } from '@/api/types/DeletePattern.type';
import type { IInventoryThread } from '@/models/Inventory.model';
import type { IGetInventoryThreadsResponse } from './types/GetInventoryThreads.type';
import type { IUpdateInventoryThreadRequest, IUpdateInventoryThreadResponse } from './types/UpdateInventoryThread.type';
import type { IPausePatternRequest, IPausePatternResponse } from './types/PausePattern.type';
import type { IUnpausePatternResponse } from './types/UnpausePattern.type';
import type { ISearchInventoryThreadsResponse } from './types/SearchInventoryThreads.type';

import { adminApi } from '@/api/parts/admin/admin.api';
import { authApi } from '@/api/parts/auth/auth.api';
import { creatorsApi } from '@/api/parts/creators/creators.api';

const auth = useAuth();

const client = axios.create({
    baseURL: '/api',
});

export const api = {

    admin: adminApi,

    auth: authApi,

    creators: creatorsApi,

    patterns: {

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

    },

    projects: {

        async getAll(): Promise<Array<IProject> | Error> {
            if (auth.details.value === null)
                return new Error('You must be logged in for this action.');

            try {
                const response = await client.get<IApiResultResponse<IGetProjectsResponse>>('/projects', {
                    headers: {
                        'Authorization': `Bearer ${auth.details.value.loginToken}`,
                    },
                });

                const projects = response.data.result.projects;

                return projects.map(projectMapper.map);
            }
            catch (error) {
                return ApiErrorMapper.map(error);
            }
        },

        async add(patternReference: string): Promise<void | Error> {
            if (auth.details.value === null)
                return new Error('You must be logged in for this action.');

            await client.post<IApiResultResponse<void>>(`/projects/${patternReference}`, {}, {
                headers: {
                    'Authorization': `Bearer ${auth.details.value.loginToken}`,
                },
            });
        },

        async get(patternReference: string): Promise<IGetProject | Error> {
            if (auth.details.value === null)
                return new Error('You must be logged in for this action.');

            try {
                const response = await client.get<IApiResultResponse<IGetProjectResponse>>(`/projects/${patternReference}`, {
                    headers: {
                        'Authorization': `Bearer ${auth.details.value.loginToken}`,
                    },
                });

                const result = response.data.result;

                return {
                    project: projectMapper.map(result.project),
                    aidaCount: result.aidaCount,
                    threads: result.threads.map(thread => ({
                        thread: patternMapper.mapThread(thread.thread),
                        stitches: thread.stitches,
                        completedStitches: thread.completedStitches.map(x => [x[0], x[1], dayjs(x[2])]),
                    })),
                };
            }
            catch (error) {
                return ApiErrorMapper.map(error);
            }
        },

        async completeStitches(patternReference: string, request: ICompleteStitchesRequest): Promise<void | Error> {
            if (auth.details.value === null)
                return new Error('You must be logged in for this action.');

            try {
                const response = await client.post<IApiResultResponse<IGetProjectResponse>>(`/projects/${patternReference}/stitches/complete`, request, {
                    headers: {
                        'Authorization': `Bearer ${auth.details.value.loginToken}`,
                    },
                });
            }
            catch (error) {
                return ApiErrorMapper.map(error);
            }
        },

        async unCompleteStitches(patternReference: string, request: ICompleteStitchesRequest): Promise<void | Error> {
            if (auth.details.value === null)
                return new Error('You must be logged in for this action.');

            try {
                const response = await client.post<IApiResultResponse<IGetProjectResponse>>(`/projects/${patternReference}/stitches/uncomplete`, request, {
                    headers: {
                        'Authorization': `Bearer ${auth.details.value.loginToken}`,
                    },

                });
            }
            catch (error) {
                return ApiErrorMapper.map(error);
            }
        },

        async pause(patternReference: string, request: IPausePatternRequest): Promise<void | Error> {
            if (auth.details.value === null)
                return new Error('You must be logged in for this action.');

            try {
                const response = await client.post<IApiResultResponse<IPausePatternResponse>>(`/projects/${patternReference}/stitches/pause`, request, {
                    headers: {
                        'Authorization': `Bearer ${auth.details.value.loginToken}`,
                    },

                });
            }
            catch (error) {
                return ApiErrorMapper.map(error);
            }
        },

        async unpause(patternReference: string): Promise<void | Error> {
            if (auth.details.value === null)
                return new Error('You must be logged in for this action.');

            try {
                const response = await client.post<IApiResultResponse<IUnpausePatternResponse>>(`/projects/${patternReference}/stitches/unpause`, {}, {
                    headers: {
                        'Authorization': `Bearer ${auth.details.value.loginToken}`,
                    },
                });
            }
            catch (error) {
                return ApiErrorMapper.map(error);
            }
        },

        async getAnalytics(patternReference: string): Promise<IGetAnalytics | Error> {
            if (auth.details.value === null)
                return new Error('You must be logged in for this action.');

            try {
                const response = await client.get<IApiResultResponse<IGetAnalyticsResponse>>(`/projects/${patternReference}/analytics`, {
                    headers: {
                        'Authorization': `Bearer ${auth.details.value.loginToken}`,
                    },

                });

                const result = response.data.result;

                return {
                    title: result.title,
                    thumbnailUrl: result.thumbnailUrl,
                    bannerImageUrl: result.bannerImageUrl,
                    purchasedAt: dayjs(result.purchasedAt),
                    totalStitches: result.totalStitches,
                    remainingStitches: result.remainingStitches,
                    completedStitches: result.completedStitches,
                    data: {
                        headings: result.data.headings,
                        values: result.data.values,
                    },
                };
            }
            catch (error) {
                return ApiErrorMapper.map(error);
            }
        },

    },

    users: {

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

    },

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
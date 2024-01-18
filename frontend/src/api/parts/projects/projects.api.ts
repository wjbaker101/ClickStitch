import dayjs from 'dayjs';

import { client } from '@/api/client';

import { useAuth } from '@/use/auth/Auth.use';
import { ApiErrorMapper } from '@/api/ApiErrorMapper';

import { patternMapper } from '@/api/mappers/Pattern.mapper';
import { projectMapper } from '@/api/mappers/Project.mapper';

import type { IApiResultResponse } from '@/api/api-models/ApiResponse.type';

import type { IGetAnalytics } from '@/models/GetAnalytics.model';
import type { IProject } from '@/models/Project.model';
import type { IGetProject } from '@/models/GetProject.model';

import type { IGetProjectsResponse } from '@/api/parts/projects/types/GetProjects.type';
import type { IGetProjectResponse } from '@/api/parts/projects/types/GetProject.type';
import type { ICompleteStitchesRequest } from '@/api/parts/projects/types/CompleteStitches.type';
import type { IGetAnalyticsResponse } from '@/api/parts/projects/types/GetAnalytics.type';
import type { IPausePatternRequest, IPausePatternResponse } from '@/api/parts/projects/types/PausePattern.type';
import type { IUnpausePatternResponse } from '@/api/parts/projects/types/UnpausePattern.type';

const auth = useAuth();

export const projectsApi = {

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

};
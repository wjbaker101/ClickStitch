import dayjs from 'dayjs';

import { apiClient } from '@/api/client';

import { patternMapper } from '@/api/mappers/Pattern.mapper';
import { projectMapper } from '@/api/mappers/Project.mapper';

import type { IGetAnalytics } from '@/models/GetAnalytics.model';
import type { IProject } from '@/models/Project.model';
import type { IGetProject } from '@/models/GetProject.model';

import type { IGetProjectsResponse } from '@/api/parts/projects/types/GetProjects.type';
import type { IGetProjectResponse } from '@/api/parts/projects/types/GetProject.type';
import type { ICompleteStitchesRequest } from '@/api/parts/projects/types/CompleteStitches.type';
import type { IGetAnalyticsResponse } from '@/api/parts/projects/types/GetAnalytics.type';
import type { IPausePatternRequest, IPausePatternResponse } from '@/api/parts/projects/types/PausePattern.type';
import type { IUnpausePatternResponse } from '@/api/parts/projects/types/UnpausePattern.type';

export const projectsApi = {

    async getAll(): Promise<Array<IProject> | Error> {
        const response = await apiClient.get<IGetProjectsResponse>({
            url: '/projects',
            auth: {
                required: true,
                use: true,
            },
        });

        if (response instanceof Error)
            return response;

        const projects = response.projects;

        return projects.map(projectMapper.map);
    },

    async add(patternReference: string): Promise<void | Error> {
        const response = await apiClient.post<void>({
            url: `/projects/${patternReference}`,
            auth: {
                required: true,
                use: true,
            },
        });

        if (response instanceof Error)
            return response;
    },

    async get(patternReference: string): Promise<IGetProject | Error> {
        const response = await apiClient.get<IGetProjectResponse>({
            url: `/projects/${patternReference}`,
            auth: {
                required: true,
                use: true,
            },
        });

        if (response instanceof Error)
            return response;

        return {
            project: projectMapper.map(response.project),
            aidaCount: response.aidaCount,
            threads: response.threads.map(thread => ({
                thread: patternMapper.mapThread(thread.thread),
                stitches: thread.stitches,
                completedStitches: thread.completedStitches.map(x => [x[0], x[1], dayjs(x[2])]),
            })),
        };
    },

    async completeStitches(patternReference: string, request: ICompleteStitchesRequest): Promise<void | Error> {
        const response = await apiClient.post<IGetProjectResponse>({
            url: `/projects/${patternReference}/stitches/complete`,
            body: request,
            auth: {
                required: true,
                use: true,
            },
        });

        if (response instanceof Error)
            return response;
    },

    async unCompleteStitches(patternReference: string, request: ICompleteStitchesRequest): Promise<void | Error> {
        const response = await apiClient.post<IGetProjectResponse>({
            url: `/projects/${patternReference}/stitches/uncomplete`,
            body: request,
            auth: {
                required: true,
                use: true,
            },
        });

        if (response instanceof Error)
            return response;
    },

    async pause(patternReference: string, request: IPausePatternRequest): Promise<void | Error> {
        const response = await apiClient.post<IPausePatternResponse>({
            url: `/projects/${patternReference}/stitches/pause`,
            body: request,
            auth: {
                required: true,
                use: true,
            },
        });

        if (response instanceof Error)
            return response;
    },

    async unpause(patternReference: string): Promise<void | Error> {
        const response = await apiClient.post<IUnpausePatternResponse>({
            url: `/projects/${patternReference}/stitches/unpause`,
            body: {},
            auth: {
                required: true,
                use: true,
            },
        });

        if (response instanceof Error)
            return response;
    },

    async getAnalytics(patternReference: string): Promise<IGetAnalytics | Error> {
        const response = await apiClient.get<IGetAnalyticsResponse>({
            url: `/projects/${patternReference}/analytics`,
            auth: {
                required: true,
                use: true,
            },
        });

        if (response instanceof Error)
            return response;

        return {
            title: response.title,
            thumbnailUrl: response.thumbnailUrl,
            bannerImageUrl: response.bannerImageUrl,
            purchasedAt: dayjs(response.purchasedAt),
            totalStitches: response.totalStitches,
            remainingStitches: response.remainingStitches,
            completedStitches: response.completedStitches,
            data: {
                headings: response.data.headings,
                values: response.data.values,
            },
        };
    },

};
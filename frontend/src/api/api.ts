import axios from 'axios';
import dayjs from 'dayjs';

import { type IAuth, useAuth } from '@/use/auth/Auth.use';
import { ApiErrorMapper } from '@/api/ApiErrorMapper';

import { paginationMapper } from '@/api/mappers/Pagination.mapper';
import { userMapper } from '@/api/mappers/User.mapper';
import { permissionMapper } from '@/api/mappers/Permission.mapper';
import { patternMapper } from '@/api/mappers/Pattern.mapper';
import { projectMapper } from '@/api/mappers/Project.mapper';
import { creatorMapper } from '@/api/mappers/Creator.mapper';
import { threadMapper } from '@/api/mappers/Thread.mapper';

import type { IApiResultResponse } from '@/api/types/ApiResponse.type';

import type { IGetCreatorPatterns } from '@/models/GetCreatorPatterns.model';
import type { IDeletePattern } from '@/models/DeletePattern.model';
import type { IPermission } from '@/models/Permission.model';
import type { IGetUsers } from '@/models/GetUsers.model';
import type { IGetAnalytics } from '@/models/GetAnalytics.model';
import type { IProject } from '@/models/Project.model';
import type { IGetProject } from '@/models/GetProject.model';
import type { IPattern } from '@/models/Pattern.model';
import type { IGetSelf } from '@/models/GetSelf.model';
import type { ICreator } from '@/models/Creator.model';
import type { IThread } from '@/models/Thread.model';

import type { ILogInRequest, ILogInResponse } from '@/api/types/LogIn.type';
import type { ICreateUserRequest, ICreateUserResponse } from '@/api/types/CreateUser.type';
import type { IAddToBasketResponse } from '@/api/types/AddToBasket.type';
import type { ISearchPatternsResponse } from '@/api/types/SearchPatterns.type';
import type { IGetProjectsResponse } from '@/api/types/GetProjects.type';
import type { IGetProjectResponse } from '@/api/types/GetProject.type';
import type { ICompleteStitchesRequest } from '@/api/types/CompleteStitches.type';
import type { IGetAnalyticsResponse } from '@/api/types/GetAnalytics.type';
import type { IGetUsersResponse } from '@/api/types/GetUsers.type';
import type { IGetSelfResponse } from '@/api/types/GetSelf.type';
import type { IGetPermissionsResponse } from '@/api/types/GetPermissions.type';
import type { IAssignPermissionToUserRequest, IAssignPermissionToUserResponse } from '@/api/types/AssignPermissionToUser.type';
import type { IRemovePermissionFromUserResponse } from '@/api/types/RemovePermissionFromUser.type';
import type { IGetSelfCreator } from '@/api/types/GetSelfCreator.type';
import type { IUpdateCreatorRequest, IUpdateCreatorResponse } from '@/api/types/UpdateCreator.type';
import type { ICreateCreatorRequest, ICreateCreatorResponse } from '@/api/types/CreateCreator.type';
import type { IGetCreatorPatternsResponse } from '@/api/types/GteCreatorPatterns.type';
import type { IUpdatePatternRequest, IUpdatePatternResponse } from '@/api/types/UpdatePattern.type';
import type { ICreatePatternRequest, ICreatePatternResponse } from '@/api/types/CreatePattern.type';
import type { IDeletePatternResponse } from '@/api/types/DeletePattern.type';
import type { ICreateThreadRequest, ICreateThreadResponse } from '@/api/types/CreateThread.type';
import type { IUpdateThreadRequest, IUpdateThreadResponse } from '@/api/types/UpdateThread.type';
import type { IDeleteThreadResponse } from '@/api/types/DeleteThread.type';
import type { IInventoryThread } from '@/models/Inventory.model';
import type { IGetInventoryThreadsResponse } from './types/GetInventoryThreads.type';
import type { IUpdateInventoryThreadRequest, IUpdateInventoryThreadResponse } from './types/UpdateInventoryThread.type';
import type { IPausePatternRequest, IPausePatternResponse } from './types/PausePattern.type';
import type { IUnpausePatternResponse } from './types/UnpausePattern.type';

const auth = useAuth();

const client = axios.create({
    baseURL: '/api',
});

export const api = {

    admin: {

        async getUsers(pageNumber: number, pageSize: number): Promise<IGetUsers | Error> {
            if (auth.details.value === null)
                return new Error('You must be logged in for this action.');

            const url = `/admin/users?page_number=${pageNumber}&page_size=${pageSize}`;

            try {
                const response = await client.get<IApiResultResponse<IGetUsersResponse>>(url, {
                    headers: {
                        'Authorization': `Bearer ${auth.details.value.loginToken}`,
                    },
                });

                const result = response.data.result;

                return {
                    users: result.users.map(x => ({
                        user: userMapper.map(x.user),
                        permissions: x.permissions.map(permissionMapper.map),
                    })),
                    pagination: paginationMapper.map(result.pagination),
                };
            }
            catch (error) {
                return ApiErrorMapper.map(error);
            }
        },

        async getPermissions(): Promise<Array<IPermission> | Error> {
            if (auth.details.value === null)
                return new Error('You must be logged in for this action.');

            try {
                const response = await client.get<IApiResultResponse<IGetPermissionsResponse>>('/admin/permissions', {
                    headers: {
                        'Authorization': `Bearer ${auth.details.value.loginToken}`,
                    },
                });

                const result = response.data.result;

                return result.permissions.map(permissionMapper.map);
            }
            catch (error) {
                return ApiErrorMapper.map(error);
            }
        },

        async assignPermissionToUser(userReference: string, request: IAssignPermissionToUserRequest): Promise<void | Error> {
            if (auth.details.value === null)
                return new Error('You must be logged in for this action.');

            const url = `/admin/users/${userReference}/permissions`;

            try {
                const response = await client.post<IApiResultResponse<IAssignPermissionToUserResponse>>(url, request, {
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

        async removePermissionFromUser(userReference: string, permissionType: number): Promise<void | Error> {
            if (auth.details.value === null)
                return new Error('You must be logged in for this action.');

            const url = `/admin/users/${userReference}/permissions/${permissionType}`;

            try {
                const response = await client.delete<IApiResultResponse<IRemovePermissionFromUserResponse>>(url, {
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

        async createThread(request: ICreateThreadRequest): Promise<IThread | Error> {
            if (auth.details.value === null)
                return new Error('You must be logged in for this action.');

            try {
                const response = await client.post<IApiResultResponse<ICreateThreadResponse>>('/admin/threads', request, {
                    headers: {
                        'Authorization': `Bearer ${auth.details.value.loginToken}`,
                    },
                });

                const thread = response.data.result.thread;

                return threadMapper.map(thread);
            }
            catch (error) {
                return ApiErrorMapper.map(error);
            }
        },

        async updateThread(threadReference: string, request: IUpdateThreadRequest): Promise<IThread | Error> {
            if (auth.details.value === null)
                return new Error('You must be logged in for this action.');

            try {
                const response = await client.put<IApiResultResponse<IUpdateThreadResponse>>(`/admin/threads/${threadReference}`, request, {
                    headers: {
                        'Authorization': `Bearer ${auth.details.value.loginToken}`,
                    },
                });

                const thread = response.data.result.thread;

                return threadMapper.map(thread);
            }
            catch (error) {
                return ApiErrorMapper.map(error);
            }
        },

        async deleteThread(threadReference: string): Promise<void | Error> {
            if (auth.details.value === null)
                return new Error('You must be logged in for this action.');

            try {
                const response = await client.post<IApiResultResponse<IDeleteThreadResponse>>(`/admin/threads/${threadReference}`, {
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

    auth: {

        async logIn(request: ILogInRequest): Promise<IAuth | Error> {
            try {
                const response = await client.post<IApiResultResponse<ILogInResponse>>('/auth/log_in', request);

                const result = response.data.result;

                return {
                    loginToken: result.loginToken,
                    email: result.email,
                    permissions: result.permissions.map(permissionMapper.map),
                    loggedInAt: dayjs(response.data.responseAt),
                };
            }
            catch (error) {
                return ApiErrorMapper.map(error);
            }
        },

    },

    basket: {

        async quickAdd(patternReference: string): Promise<void | Error> {
            if (auth.details.value === null)
                return new Error('You must be logged in for this action.');

            await client.post<IApiResultResponse<IAddToBasketResponse>>(`/basket/item/${patternReference}/quick`, {}, {
                headers: {
                    'Authorization': `Bearer ${auth.details.value.loginToken}`,
                },
            });
        },

    },

    creators: {

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
    },

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

            await client.post<IApiResultResponse<IAddToBasketResponse>>(`/projects/${patternReference}`, {}, {
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
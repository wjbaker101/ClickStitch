import axios from 'axios';
import dayjs from 'dayjs';

import { IAuth, useAuth } from '@/use/auth/Auth.use';
import { ApiErrorMapper } from '@/api/ApiErrorMapper';

import { IApiResultResponse } from '@/api/types/ApiResponse.type';

import { ILogInRequest, ILogInResponse } from '@/api/types/LogIn.type';

import { ICreateUserRequest, ICreateUserResponse } from '@/api/types/CreateUser.type';

import { IBasket } from '@/models/Basket.model';
import { IGetBasketResponse } from '@/api/types/GetBasket.type';
import { IAddToBasketResponse } from '@/api/types/AddToBasket.type';
import { IRemoveFromBasketResponse } from '@/api/types/RemoveFromBasket.type';

import { patternMapper } from '@/api/mappers/Pattern.mapper';
import { IPattern } from '@/models/Pattern.model';
import { ISearchPatternsResponse } from '@/api/types/SearchPatterns.type';

import { projectMapper } from '@/api/mappers/Project.mapper';
import { IProject } from '@/models/Project.model';
import { IGetProject } from '@/models/GetProject.model';
import { IGetProjectsResponse } from '@/api/types/GetProjects.type';
import { IGetProjectResponse } from '@/api/types/GetProject.type';
import { ICompleteStitchesRequest } from './types/CompleteStitches.type';
import { IGetAnalytics } from '@/models/GetAnalytics.model';
import { IGetAnalyticsResponse } from './types/GetAnalytics.type';
import { IGetUsersResponse } from './types/GetUsers.type';
import { IGetUsers } from '@/models/GetUsers.model';
import { paginationMapper } from './mappers/Pagination.mapper';
import { IUser } from '@/models/User.model';
import { IGetSelfResponse } from './types/GetSelf.type';
import { userMapper } from './mappers/User.mapper';
import { permissionMapper } from './mappers/Permission.mapper';
import { IPermission } from '@/models/Permission.model';
import { IGetPermissionsResponse } from './types/GetPermissions.type';
import { IAssignPermissionToUserRequest, IAssignPermissionToUserResponse } from './types/AssignPermissionToUser.type';
import { IRemovePermissionFromUserResponse } from './types/RemovePermissionFromUser.type';
import { IGetSelf } from '@/models/GetSelf.model';
import { IGetSelfCreator } from './types/GetSelfCreator.type';
import { ICreator } from '@/models/Creator.model';
import { creatorMapper } from './mappers/Creator.mapper';
import { IUpdateCreatorRequest, IUpdateCreatorResponse } from './types/UpdateCreator.type';
import { ICreateCreatorRequest, ICreateCreatorResponse } from './types/CreateCreator.type';
import { IGetCreatorPatternsResponse } from './types/GteCreatorPatterns.type';
import { IGetCreatorPatterns } from '@/models/GetCreatorPatterns.model';
import { IUpdatePatternRequest, IUpdatePatternResponse } from './types/UpdatePattern.type';
import { IDeletePatternResponse } from './types/DeletePattern.type';
import { IDeletePattern } from '@/models/DeletePattern.model';

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

        async get(): Promise<IBasket | Error> {
            if (auth.details.value === null)
                return new Error('You must be logged in for this action.');

            try {
                const response = await client.get<IApiResultResponse<IGetBasketResponse>>('/basket', {
                    headers: {
                        'Authorization': `Bearer ${auth.details.value.loginToken}`,
                    },
                });

                const basket = response.data.result.basket;

                return {
                    items: basket.items.map(x =>({
                        pattern: patternMapper.map(x.pattern),
                        addedAt: dayjs(x.addedAt),
                    })),
                    totalPrice: basket.totalPrice,
                };
            }
            catch (error) {
                return ApiErrorMapper.map(error);
            }
        },

        async addItem(patternReference: string): Promise<void | Error> {
            if (auth.details.value === null)
                return new Error('You must be logged in for this action.');

            await client.post<IApiResultResponse<IAddToBasketResponse>>(`/basket/item/${patternReference}`, {}, {
                headers: {
                    'Authorization': `Bearer ${auth.details.value.loginToken}`,
                },
            });
        },

        async removeItem(patternReference: string): Promise<void> {
            if (auth.details.value === null)
                return;

            await client.delete<IApiResultResponse<IRemoveFromBasketResponse>>(`/basket/item/${patternReference}`, {
                headers: {
                    'Authorization': `Bearer ${auth.details.value.loginToken}`,
                },
            });
        },

        async quickAdd(patternReference: string): Promise<void | Error> {
            if (auth.details.value === null)
                return new Error('You must be logged in for this action.');

            await client.post<IApiResultResponse<IAddToBasketResponse>>(`/basket/item/${patternReference}/quick`, {}, {
                headers: {
                    'Authorization': `Bearer ${auth.details.value.loginToken}`,
                },
            });
        },

        async complete() {
            if (auth.details.value === null)
                return new Error('You must be logged in for this action.');

            await client.post<IApiResultResponse<IAddToBasketResponse>>('/basket/complete', {}, {
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

};
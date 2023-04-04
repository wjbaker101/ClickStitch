import axios from 'axios';
import dayjs from 'dayjs';

import { useAuth } from '@/use/auth/Auth.use';
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

const auth = useAuth();

const client = axios.create({
    baseURL: '/api',
});

export const api = {

    auth: {

        async logIn(request: ILogInRequest): Promise<ILogInResponse | Error> {
            try {
                const response = await client.post<IApiResultResponse<ILogInResponse>>('/auth/log_in', request);

                const result = response.data.result;

                return result;
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

    patterns: {

        async search(): Promise<Array<IPattern>> {
            if (auth.details.value === null)
                return [];

            const response = await client.get<IApiResultResponse<ISearchPatternsResponse>>('/patterns', {
                headers: {
                    'Authorization': `Bearer ${auth.details.value.loginToken}`,
                },
            });

            const patterns = response.data.result.patterns;

            return patterns.map(patternMapper.map);
        },

    },

    projects: {

        async getAll(): Promise<Array<IProject>> {
            if (auth.details.value === null)
                return [];

            const response = await client.get<IApiResultResponse<IGetProjectsResponse>>('/projects', {
                headers: {
                    'Authorization': `Bearer ${auth.details.value.loginToken}`,
                },
            });

            const projects = response.data.result.projects;

            return projects.map(projectMapper.map);
        },

        async get(patternReference: string): Promise<IGetProject | Error> {
            if (auth.details.value === null)
                return new Error('You must be logged in for this action.');

            const response = await client.get<IApiResultResponse<IGetProjectResponse>>(`/projects/${patternReference}`, {
                headers: {
                    'Authorization': `Bearer ${auth.details.value.loginToken}`,
                },
            });

            const result = response.data.result;

            return {
                project: projectMapper.map(result.project),
                aidaCount: result.aidaCount,
                stitches: result.stitches.map(patternMapper.mapStitch),
                threads: result.threads.map(patternMapper.mapThread),
            };
        },

    },

    users: {

        async createUser(request: ICreateUserRequest): Promise<ICreateUserResponse> {
            const response = await client.post<IApiResultResponse<ICreateUserResponse>>('/users', request);

            return {};
        },

    },

};
import axios from 'axios';

import { useAuth } from '@/use/auth/Auth.use';

import type { IApiErrorResponse, IApiResultResponse } from '@/api/api-models/ApiResponse.type';

export const client = axios.create({
    baseURL: '/api',
});


interface IRequestOptions {
    readonly method: 'get' | 'post' | 'put' | 'delete';
    readonly url: string;
    readonly body?: any;
    readonly queryParams?: Map<string, string>;
    readonly auth: {
        readonly use: boolean;
        readonly required: boolean;
    },
}

const auth = useAuth();

const doRequest = async <TResult>(options: IRequestOptions): Promise<TResult | Error> => {

    if (options.auth.required && auth.details.value === null)
        return new Error('You must be logged in for this action.');

    try {
        const builtUrl = new URL(`/api${options.url}`, window.location.origin);

        if (options.queryParams !== undefined) {
            for (const [key, value] of options.queryParams.entries()) {
                if (value === null)
                    continue;

                builtUrl.searchParams.set(key, value);
            }
        }

        const headers: HeadersInit = {};

        if (options.auth.use && auth.details.value !== null) {
            headers['Authorization'] = `Bearer ${auth.details.value.loginToken}`;
            headers['Content-Type'] = 'application/json';
        }

        const response = await fetch(builtUrl, {
            method: options.method,
            body: JSON.stringify(options.body),
            headers,
        });

        if (!response.ok) {
            const errorResponse = await response.json() as IApiErrorResponse;
            return new Error(errorResponse.failureMessage);
        }

        const result = await response.json() as IApiResultResponse<TResult>;

        return result.result;
    }
    catch (error) {
        console.log(error);
        return new Error('Unable to retrieve data from server, please try again later.');
    }
};

export const apiClient = {

    async get<TResult>(options: Omit<IRequestOptions, 'method' | 'body'>): Promise<TResult | Error> {
        return await doRequest<TResult>({
            method: 'get',
            ...options,
        });
    },

    async post<TResult>(options: Omit<IRequestOptions, 'method'>): Promise<TResult | Error> {
        return await doRequest<TResult>({
            method: 'post',
            ...options,
        });
    },

    async put<TResult>(options: Omit<IRequestOptions, 'method'>): Promise<TResult | Error> {
        return await doRequest<TResult>({
            method: 'put',
            ...options,
        });
    },

    async delete<TResult>(options: Omit<IRequestOptions, 'method' | 'body'>): Promise<TResult | Error> {
        return await doRequest<TResult>({
            method: 'delete',
            ...options,
        });
    },

};
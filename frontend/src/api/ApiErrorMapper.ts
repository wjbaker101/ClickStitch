import { AxiosError } from 'axios';

import { useAuth } from '@/use/auth/Auth.use';

import { type IApiErrorResponse } from '@/api/api-models/ApiResponse.type';

const auth = useAuth();

export const ApiErrorMapper = {

    map(input: any): Error {
        const axiosError = input as AxiosError;

        if (axiosError.response) {
            if (axiosError.response.status === 401) {
                auth.clear();
                return new Error('Unable to authenticate user, redirecting back to the login page.');
            }

            const response = axiosError.response.data as IApiErrorResponse;

            return new Error(response.failureMessage);
        }
        if (axiosError.request) {
            return new Error('Something went wrong during request. Please refresh and try again.');
        }

        return new Error('Something went wrong preparing request. Please refresh and try again.');
    },

};
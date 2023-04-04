import { AxiosError } from 'axios';

import { IApiErrorResponse } from '@/api/types/ApiResponse.type';

export const ApiErrorMapper = {

    map(input: any): Error {
        const axiosError = input as AxiosError;

        if (axiosError.response) {
            const response = axiosError.response.data as IApiErrorResponse;

            return new Error(response.failureMessage);
        }
        if (axiosError.request) {
            return new Error('Something went wrong during request. Please refresh and try again.');
        }

        return new Error('Something went wrong preparing request. Please refresh and try again.');
    },

};
import type { IApiThread } from '@/api/api-models/ApiThread.type';

export interface IGetThreadsByColourRequest {
    readonly colours: Array<{
        readonly r: number;
        readonly g: number;
        readonly b: number;
    }>;
}

export interface IGetThreadsByColourResponse {
    readonly threads: Array<IApiThread>;
}
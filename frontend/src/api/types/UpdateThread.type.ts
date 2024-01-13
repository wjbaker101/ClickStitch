import type { IApiThread } from '@/api/api-models/ApiThread.type';

export interface IUpdateThreadRequest {
    readonly code: string;
    readonly description: string;
}

export interface IUpdateThreadResponse {
    readonly thread: IApiThread;
}
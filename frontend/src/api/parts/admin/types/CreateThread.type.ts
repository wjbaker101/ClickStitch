import type { IApiThread } from '@/api/api-models/ApiThread.type';

export interface ICreateThreadRequest {
    readonly code: string;
    readonly description: string;
}

export interface ICreateThreadResponse {
    readonly thread: IApiThread;
}
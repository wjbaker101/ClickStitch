import type { IApiThread } from './ApiThread.type';

export interface ICreateThreadRequest {
    readonly code: string;
    readonly description: string;
}

export interface ICreateThreadResponse {
    readonly thread: IApiThread;
}
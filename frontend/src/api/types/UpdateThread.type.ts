import type { IApiThread } from './ApiThread.type';

export interface IUpdateThreadRequest {
    readonly code: string;
    readonly description: string;
}

export interface IUpdateThreadResponse {
    readonly thread: IApiThread;
}
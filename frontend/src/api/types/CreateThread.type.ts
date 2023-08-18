import type { IApiPatternThread } from './ApiPattern.type';

export interface ICreateThreadRequest {
    readonly code: string;
    readonly description: string;
}

export interface ICreateThreadResponse {
    readonly thread: IApiPatternThread;
}
import type { IApiPattern } from '@/api/api-models/ApiPattern.type';

export interface IGetPatternResponse {
    readonly pattern: IApiPattern;
}
import { type IApiPattern } from '@/api/api-models/ApiPattern.type';

export interface ISearchPatternsResponse {
    readonly patterns: Array<IApiPattern>;
}
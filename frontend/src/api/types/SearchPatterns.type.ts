import { type IApiPattern } from '@/api/types/ApiPattern.type';

export interface ISearchPatternsResponse {
    readonly patterns: Array<IApiPattern>;
}
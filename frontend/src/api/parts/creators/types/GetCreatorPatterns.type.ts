import { type IApiPagination } from '@/api/api-models/ApiPagination.type';
import { type IApiPattern } from '@/api/api-models/ApiPattern.type';

export interface ISearchCreatorPatternsResponse {
    readonly patterns: Array<IApiPattern>;
    readonly projectPatternReferencesForUser: Array<string>;
    readonly pagination: IApiPagination;
}
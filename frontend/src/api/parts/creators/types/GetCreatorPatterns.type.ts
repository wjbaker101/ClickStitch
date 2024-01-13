import { type IApiPagination } from '@/api/api-models/ApiPagination.type';
import { type IApiPattern } from '@/api/api-models/ApiPattern.type';

export interface IGetCreatorPatternsResponse {
    readonly patterns: Array<IApiPattern>;
    readonly pagination: IApiPagination;
}
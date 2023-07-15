import { type IApiPagination } from './ApiPagination.type';
import { type IApiPattern } from './ApiPattern.type';

export interface IGetCreatorPatternsResponse {
    readonly patterns: Array<IApiPattern>;
    readonly pagination: IApiPagination;
}
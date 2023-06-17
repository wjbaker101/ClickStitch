import { IApiPagination } from './ApiPagination.type';
import { IApiPattern } from './ApiPattern.type';

export interface IGetCreatorPatternsResponse {
    readonly patterns: Array<IApiPattern>;
    readonly pagination: IApiPagination;
}
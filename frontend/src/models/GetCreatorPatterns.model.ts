import { type IPagination } from './Pagination.model';
import { type IPattern } from './Pattern.model';

export interface ISearchCreatorPatterns {
    readonly patterns: Array<IPattern>;
    readonly pagination: IPagination;
}
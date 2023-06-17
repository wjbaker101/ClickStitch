import { IPagination } from './Pagination.model';
import { IPattern } from './Pattern.model';

export interface IGetCreatorPatterns {
    readonly patterns: Array<IPattern>;
    readonly pagination: IPagination;
}
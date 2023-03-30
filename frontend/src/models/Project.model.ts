import { Dayjs } from 'dayjs';

import { IPattern } from './Pattern.model';

export interface IProject {
    readonly pattern: IPattern;
    readonly purchasedAt: Dayjs;
}
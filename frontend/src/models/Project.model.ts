import { Dayjs } from 'dayjs';

import { IPattern } from '@/models/Pattern.model';

export interface IProject {
    readonly pattern: IPattern;
    readonly purchasedAt: Dayjs;
}
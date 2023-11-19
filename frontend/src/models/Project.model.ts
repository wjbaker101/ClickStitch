import { Dayjs } from 'dayjs';

import { type IPattern } from '@/models/Pattern.model';

export interface IProject {
    readonly pattern: IPattern;
    readonly purchasedAt: Dayjs;
    pausePositionX: number | null;
    pausePositionY: number | null;
}
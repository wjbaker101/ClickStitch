import { Dayjs } from 'dayjs';

export interface IGetAnalytics {
    readonly purchasedAt: Dayjs;
    readonly totalStitches: number;
    readonly completedStitches: number;
    readonly remainingStitches: number;
}
import { Dayjs } from 'dayjs';

export interface IGetAnalytics {
    readonly title: string;
    readonly thumbnailUrl: string | null;
    readonly purchasedAt: Dayjs;
    readonly totalStitches: number;
    readonly completedStitches: number;
    readonly remainingStitches: number;
}
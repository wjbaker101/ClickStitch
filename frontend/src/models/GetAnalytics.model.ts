import { Dayjs } from 'dayjs';

export interface IGetAnalytics {
    readonly title: string;
    readonly thumbnailUrl: string | null;
    readonly bannerImageUrl: string;
    readonly purchasedAt: Dayjs;
    readonly totalStitches: number;
    readonly completedStitches: number;
    readonly remainingStitches: number;
    readonly data: {
        readonly headings: Array<string>;
        readonly values: Array<number>;
    };
}
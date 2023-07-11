export interface IGetAnalyticsResponse {
    readonly title: string;
    readonly thumbnailUrl: string | null;
    readonly bannerImageUrl: string;
    readonly purchasedAt: string;
    readonly totalStitches: number;
    readonly completedStitches: number;
    readonly remainingStitches: number;
    readonly data: {
        readonly headings: Array<string>;
        readonly values: Array<number>;
    };
}
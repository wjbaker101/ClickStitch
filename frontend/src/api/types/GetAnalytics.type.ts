export interface IGetAnalyticsResponse {
    readonly title: string;
    readonly thumbnailUrl: string | null;
    readonly purchasedAt: string;
    readonly totalStitches: number;
    readonly completedStitches: number;
    readonly remainingStitches: number;
}
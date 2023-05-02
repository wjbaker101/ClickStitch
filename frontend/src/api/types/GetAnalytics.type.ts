export interface IGetAnalyticsResponse {
    readonly purchasedAt: string;
    readonly totalStitches: number;
    readonly completedStitches: number;
    readonly remainingStitches: number;
}
export interface ICompleteStitchesRequest {
    readonly positions: Array<{
        readonly x: number;
        readonly y: number;
    }>;
}

export interface ICompleteStitchesResponse {
}
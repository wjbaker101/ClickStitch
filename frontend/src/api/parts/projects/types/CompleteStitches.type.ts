export interface ICompleteStitchesRequest {
    readonly stitchesByThread: Record<number, Array<IPosition>>;
}

export interface IPosition {
    readonly x: number;
    readonly y: number;
}

export interface ICompleteStitchesResponse {
}
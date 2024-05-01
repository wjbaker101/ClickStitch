export interface ICompleteBackStitchesRequest {
    readonly backStitchesByThread: Record<number, Array<IPosition>>;
}

export interface IPosition {
    readonly startX: number;
    readonly startY: number;
    readonly endX: number;
    readonly endY: number;
}

export interface ICompleteBackStitchesResponse {
}
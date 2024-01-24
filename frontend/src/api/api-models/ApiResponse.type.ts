interface IApiResponse {
    readonly responseAt: string;
}

export interface IApiResultResponse<T> extends IApiResponse {
    readonly result: T;
}

export interface IApiErrorResponse extends IApiResponse {
    readonly errorMessage: string;
}
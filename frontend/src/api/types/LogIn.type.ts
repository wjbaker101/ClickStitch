export interface ILogInRequest {
    readonly username: string;
    readonly password: string;
}

export interface ILogInResponse {
    readonly loginToken: string;
}
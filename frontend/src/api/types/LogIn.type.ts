export interface ILogInRequest {
    readonly email: string;
    readonly password: string;
}

export interface ILogInResponse {
    readonly loginToken: string;
    readonly email: string;
}
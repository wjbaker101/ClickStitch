export interface IApiUser {
    readonly reference: string;
    readonly createdAt: string;
    readonly email: string;
    readonly lastLoginAt: string | null;
}
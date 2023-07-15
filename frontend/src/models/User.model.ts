import { Dayjs } from 'dayjs';

export interface IUser {
    readonly reference: string;
    readonly createdAt: Dayjs;
    readonly email: string;
    readonly lastLoginAt: Dayjs;
}
import { Dayjs } from 'dayjs';

export interface ICreator {
    readonly reference: string;
    readonly createdAt: Dayjs;
    readonly name: string;
    readonly storeUrl: string;
}
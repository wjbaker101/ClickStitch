import { Dayjs } from 'dayjs';

export interface IPattern {
    readonly reference: string;
    readonly createdAt: Dayjs;
    readonly title: string;
    readonly width: number;
    readonly height: number;
    readonly price: number;
}
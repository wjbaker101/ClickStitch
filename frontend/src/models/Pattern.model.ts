import { Dayjs } from 'dayjs';

export interface IPattern {
    readonly reference: string;
    readonly createdAt: Dayjs;
    readonly title: string;
    readonly width: number;
    readonly height: number;
    readonly price: number;
}

export interface IStitch {
    readonly threadIndex: number;
    readonly x: number;
    readonly y: number;
}

export interface IThread {
    readonly index: number;
    readonly name: string;
    readonly description: string;
    readonly colour: string;
}
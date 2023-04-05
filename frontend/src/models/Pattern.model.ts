import { Dayjs } from 'dayjs';

export interface IPattern {
    readonly reference: string;
    readonly createdAt: Dayjs;
    readonly title: string;
    readonly width: number;
    readonly height: number;
    readonly price: number;
    readonly thumbnailUrl: string;
    readonly threadCount: number;
    readonly stitchCount: number;
}

export interface IStitch {
    readonly threadIndex: number;
    readonly x: number;
    readonly y: number;
    stitchedAt: Dayjs | null;
}

export interface IThread {
    readonly index: number;
    readonly name: string;
    readonly description: string;
    readonly colour: string;
}
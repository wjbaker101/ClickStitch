export interface IApiPattern {
    readonly reference: string;
    readonly createdAt: string;
    readonly title: string;
    readonly width: number;
    readonly height: number;
    readonly price: number;
}

export interface IApiStitch {
    readonly threadIndex: number;
    readonly x: number;
    readonly y: number;
}

export interface IApiThread {
    readonly index: number;
    readonly name: string;
    readonly description: string;
    readonly colour: string;
}
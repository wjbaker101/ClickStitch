export interface IApiPattern {
    readonly reference: string;
    readonly createdAt: string;
    readonly title: string;
    readonly width: number;
    readonly height: number;
    readonly price: number;
    readonly thumbnailUrl: string;
    readonly threadCount: number;
    readonly stitchCount: number;
}

export interface IApiStitch {
    readonly threadIndex: number;
    readonly x: number;
    readonly y: number;
    readonly stitchedAt: string | null;
}

export interface IApiThread {
    readonly index: number;
    readonly name: string;
    readonly description: string;
    readonly colour: string;
}
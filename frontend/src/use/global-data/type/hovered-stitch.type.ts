export interface IHoveredStitch {
    x: number;
    y: number;
    thread: {
        index: number;
        name: string;
        description: string;
        colour: string;
    };
    isDone: boolean;
}
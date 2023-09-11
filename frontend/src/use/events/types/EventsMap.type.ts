export interface IEventsMap {
    readonly 'JumpToStitch': IJumpToStitchEvent;
}

export type EventNames = keyof IEventsMap;

export interface IJumpToStitchEvent {
    readonly x: number;
    readonly y: number;
}
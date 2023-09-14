import type { IThreadDetails } from '@/models/GetProject.model';

export interface IEventsMap {
    readonly 'JumpToStitch': IJumpToStitchEvent;
    readonly 'StartJumpToStitches': IStartJumpToStitchesEvent;
    readonly 'EndJumpToStitches': IEndJumpToStitchesEvent;
}

export type EventNames = keyof IEventsMap;

export interface IJumpToStitchEvent {
    readonly x: number;
    readonly y: number;
}

export interface IStartJumpToStitchesEvent {
    readonly thread: IThreadDetails;
}

export interface IEndJumpToStitchesEvent {
}
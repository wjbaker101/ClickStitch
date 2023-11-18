import type { IContextMenuSchema } from '@/components/context-menu/types/ContextMenuSchema.type';
import type { IThreadDetails } from '@/models/GetProject.model';

export interface IEventsMap {
    readonly 'JumpToStitch': IJumpToStitchEvent;
    readonly 'StartJumpToStitches': IStartJumpToStitchesEvent;
    readonly 'EndJumpToStitches': IEndJumpToStitchesEvent;
    readonly 'OpenContextMenu': IOpenContextMenuEvent;
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

export interface IOpenContextMenuEvent {
    readonly x: number;
    readonly y: number;
    readonly schema: IContextMenuSchema;
}
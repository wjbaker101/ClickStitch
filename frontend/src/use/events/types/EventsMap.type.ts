import type { IContextMenuSchema } from '@/components/context-menu/types/ContextMenuSchema.type';
import type { IThreadDetails } from '@/models/GetProject.model';

export interface IEventsMap {
    readonly 'JumpToStitch': IJumpToStitchEvent;
    readonly 'StartJumpToStitches': IStartJumpToStitchesEvent;
    readonly 'EndJumpToStitches': IEndJumpToStitchesEvent;
    readonly 'OpenContextMenu': IOpenContextMenuEvent;
    readonly 'CloseContextMenu': ICloseContextMenuEvent;
    readonly 'PatternDoubleClick': IPatternDoubleClickEvent;
    readonly 'GoToPausePosition': IGoToPausePositionEvent;
    readonly 'HighlightThread': IHighlightThreadEvent;
}

export type EventNames = keyof IEventsMap;

export interface IJumpToStitchEvent {
    readonly x: number;
    readonly y: number;
    readonly endX?: number;
    readonly endY?: number;
    readonly type: 'stitch' | 'back-stitch';
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

export interface ICloseContextMenuEvent {
}

export interface IPatternDoubleClickEvent {
}

export interface IGoToPausePositionEvent {
}

export interface IHighlightThreadEvent {
    readonly threadIndex: number;
}
import { Dayjs } from 'dayjs';

import { type IPatternThread } from '@/models/Pattern.model';
import { type IProject } from '@/models/Project.model';

export interface IGetProject {
    readonly project: IProject;
    readonly aidaCount: number;
    readonly threads: Array<IThreadDetails>;
}

export interface IThreadDetails {
    readonly thread: IPatternThread;
    readonly stitches: Array<[number, number]>;
    readonly completedStitches: Array<[number, number, Dayjs]>;
    readonly backStitches: Array<[number, number, number, number]>;
    readonly completedBackStitches: Array<[number, number, number, number, Dayjs]>;
}
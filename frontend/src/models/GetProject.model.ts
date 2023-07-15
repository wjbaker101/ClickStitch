import { Dayjs } from 'dayjs';

import { type IThread } from '@/models/Pattern.model';
import { type IProject } from '@/models/Project.model';

export interface IGetProject {
    readonly project: IProject;
    readonly aidaCount: number;
    readonly threads: Array<IThreadDetails>;
}

interface IThreadDetails {
    readonly thread: IThread;
    readonly stitches: Array<[number, number]>;
    readonly completedStitches: Array<[number, number, Dayjs]>;
}
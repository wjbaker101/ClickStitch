import { IThread } from '@/models/Pattern.model';
import { IProject } from '@/models/Project.model';

export interface IGetProject {
    readonly project: IProject;
    readonly aidaCount: number;
    readonly threads: Array<IThreadDetails>;
}

interface IThreadDetails {
    readonly thread: IThread;
    readonly stitches: Array<[number, number]>;
}
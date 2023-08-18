import { type IApiPatternThread } from '@/api/types/ApiPattern.type';
import { type IApiProject } from '@/api/types/ApiProject.type';

export interface IGetProjectResponse {
    readonly project: IApiProject;
    readonly aidaCount: number;
    readonly threads: Array<IThreadDetails>;
}

interface IThreadDetails {
    readonly thread: IApiPatternThread;
    readonly stitches: Array<[number, number]>;
    readonly completedStitches: Array<[number, number, string]>;
}
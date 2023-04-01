import { IStitch, IThread } from '@/models/Pattern.model';
import { IProject } from '@/models/Project.model';

export interface IGetProject {
    readonly project: IProject;
    readonly aidaCount: number;
    readonly stitches: Array<IStitch>;
    readonly threads: Array<IThread>;
}
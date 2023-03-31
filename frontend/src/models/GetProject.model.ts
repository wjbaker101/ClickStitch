import { IStitch, IThread } from './Pattern.model';
import { IProject } from './Project.model';

export interface IGetProject {
    readonly project: IProject;
    readonly stitches: Array<IStitch>;
    readonly threads: Array<IThread>;
}
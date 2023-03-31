import { IApiStitch, IApiThread } from './ApiPattern.type';
import { IApiProject } from './ApiProject.type';

export interface IGetProjectResponse {
    readonly project: IApiProject;
    readonly stitches: Array<IApiStitch>;
    readonly threads: Array<IApiThread>;
}
import { IApiStitch, IApiThread } from '@/api/types/ApiPattern.type';
import { IApiProject } from '@/api/types/ApiProject.type';

export interface IGetProjectResponse {
    readonly project: IApiProject;
    readonly stitches: Array<IApiStitch>;
    readonly threads: Array<IApiThread>;
}
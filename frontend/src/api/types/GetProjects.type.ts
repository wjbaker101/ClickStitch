import { IApiProject } from './ApiProject.type';

export interface IGetProjectsResponse {
    readonly projects: Array<IApiProject>;
}
import { IApiProject } from '@/api/types/ApiProject.type';

export interface IGetProjectsResponse {
    readonly projects: Array<IApiProject>;
}
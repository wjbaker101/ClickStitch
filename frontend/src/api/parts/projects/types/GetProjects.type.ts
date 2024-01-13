import { type IApiProject } from '@/api/api-models/ApiProject.type';

export interface IGetProjectsResponse {
    readonly projects: Array<IApiProject>;
}
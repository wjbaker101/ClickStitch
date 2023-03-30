import { IApiPattern } from './ApiPattern.type';

export interface IApiProject {
    readonly pattern: IApiPattern;
    readonly purchasedAt: string;
}
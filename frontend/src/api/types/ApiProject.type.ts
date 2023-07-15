import { type IApiPattern } from '@/api/types/ApiPattern.type';

export interface IApiProject {
    readonly pattern: IApiPattern;
    readonly purchasedAt: string;
}
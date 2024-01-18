import { type IApiPattern } from '@/api/api-models/ApiPattern.type';

export interface IApiProject {
    readonly pattern: IApiPattern;
    readonly purchasedAt: string;
    readonly pausePositionX: number | null;
    readonly pausePositionY: number | null;
}